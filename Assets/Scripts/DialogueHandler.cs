using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using Yarn;
using Yarn.Saliency;
using static Unity.Collections.AllocatorManager;

namespace Yarn.Unity
{
    public class DialogueHandler : MonoBehaviour
    {
        bool lockVisited;
        public DialogueRunner dr;
        public GameObject dialogueSystem;
        public GameObject endScreen;
        public List<GameObject> disabledDuringDialogue;
        private bool isDialogueActive;
        public bool allowClicks = true;
        public Animator spriteAnimator;
        public Image portrait;
        public Image portrait2;
        public Image portrait3;
        public Sprite[] vex;
        public Sprite[] robogirl;
        public AudioClip[] cgmusic;
        
        public GameObject characterName;
        maintenance ma;
        soundEffects se;
        minigameHandler mh;

        public Image cgbackground;
        public Image blackScreen;
        public Sprite[] cgImages;

        int tereseEnding = 0;
        int eliseEnding = 0;

        public AudioSource endingMusic;
        public AudioSource CGmusic;
        public AudioSource dialogueMusic;
        bool firstTime3 = true;


        // Start is called before the first frame update
        void Start()
        {

            characterName.gameObject.SetActive(true);
            ma = GameObject.Find("scriptholder").gameObject.GetComponent<maintenance>();
            se = GameObject.Find("scriptholder").gameObject.GetComponent<soundEffects>();
            mh = GameObject.Find("scriptholder").gameObject.GetComponent<minigameHandler>();
            //vs = GameObject.FindFirstObjectByType<InMemoryVariableStorage>();
            dr.AddCommandHandler<string>("setVex", setVex);
            dr.AddCommandHandler<string>("setRobo", setRobo);
            dr.AddCommandHandler<string>("setElise", setElise);
            dr.AddCommandHandler<string>("noName", noName);
            dr.AddCommandHandler<string>("hideSprites", hideSprites);
            dr.AddCommandHandler<string>("endDialogue", EndDialogue);
            dr.AddCommandHandler<string>("playEndingMusic", playEndingMusic);
            dr.AddCommandHandler<int>("runMaintenance", runMaintenance);
            dr.AddCommandHandler<int>("playSound", playSound);
            dr.AddCommandHandler<int>("showCG", showCG);
            dr.AddCommandHandler<int>("hideCG", hideCG);
            dr.AddCommandHandler<int>("stopCGmusic", stopCGmusic);
            dr.AddCommandHandler<int>("pickChoice1", pickChoice1);
            dr.AddCommandHandler<int>("choice1", choice1);
            dr.AddCommandHandler<int>("handleFade", handleFade);
            dr.AddCommandHandler<string>("fadeMusicIn", fadeMusicIn);
            dr.AddCommandHandler<string>("fadeMusicOut", fadeMusicOut);
            print("adding commands");
            //runNode("Cutscene1");
        }

        public void addCommands()
        {
            dr.AddCommandHandler<string>("setVex", setVex);
            dr.AddCommandHandler<string>("setRobo", setRobo);
            dr.AddCommandHandler<string>("noName", noName);
        }

        public void fadeMusicIn(string musicName)
        {
            AudioSource music = new AudioSource();
            if(musicName == "cg")
            {
                print("playing cg music");
                music = CGmusic;
            }
            if (musicName == "dialogue")
            {
                music = dialogueMusic;
            }
            if (musicName == "ending")
            {
                music = endingMusic;
            }
            StartCoroutine(FadeIn(music, 3f));
        }

        public void fadeMusicOut(string musicName)
        {
            AudioSource music = new AudioSource();
            if (musicName == "cg")
            {
                
                music = CGmusic;
            }
            if (musicName == "dialogue")
            {
                music = dialogueMusic;
            }
            if (musicName == "ending")
            {
                music = endingMusic;
            }
            StartCoroutine(FadeOut(music, 2f));
        }
        public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            audioSource.volume = 0f;
            audioSource.Play();
            while (audioSource.volume < 0.3)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            
            audioSource.volume = startVolume;
        }
        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
        public void playEndingMusic(string mode)
        {
            if(mode == "play")
            {
                endingMusic.Play();
            }
            else
            {
                endingMusic.Stop();
            }
        }




        // Update is called once per frame
        void Update()
        {

        }
        public void showCG(int num)
        {
            /*
             * 0 - background
             * 1 - violin
             * 2 - vexel15e face touch
             * 3 - flashback1
             * 4 - flashback2
             * 5 - vexel15e hugging
             * 6 - graduation
             * 7 - flashback promise1
             * 8 - flashback promise2
             * 9-11 - endings 1-3
             */
            cgbackground.gameObject.SetActive(true);
            cgbackground.gameObject.GetComponent<Image>().sprite = cgImages[num];  

            if(num == 0)
            {  
                fadeMusicOut("cg");
                //dialogueMusic.Play();
            }

            if (num == 1)
            {
                CGmusic.clip = cgmusic[0];
                CGmusic.Play();

                //fadeMusicIn("cg");
            }
            if(num == 2)
            {
                CGmusic.clip = cgmusic[1];
                CGmusic.Play();
            }
            if (num == 3 && firstTime3 == true)
            {
                CGmusic.clip = cgmusic[2];
                fadeMusicIn("cg");
                CGmusic.Play();
                firstTime3 = false;
            }
            if (num == 5 || num == 6)
            {
                CGmusic.clip = cgmusic[3];
                CGmusic.Play();
            }
            else
            {
                return;
            }
            
        }

        public void stopCGmusic(int num)
        {
            CGmusic.Stop();
            //fadeMusicIn("dialogue");
        }
        

        public void hideCG(int num)
        {
            cgbackground.gameObject.SetActive(false);
        }

        public void testFunction()
        {
            print("testing");
        }
        public void playSound(int num)
        {
            se.switchSound(num);
        }
        public void setRobo(string spriteName)
        {
            characterName.GetComponent<TMPro.TextMeshProUGUI>().text = "EL1-5E";
            characterName.gameObject.SetActive(true);
            portrait2.gameObject.SetActive(true);
            portrait2.gameObject.GetComponent<Image>().enabled = true;
            portrait3.gameObject.SetActive(false);
            if (cgbackground.gameObject.activeSelf == true)
            {
                cgbackground.gameObject.SetActive(false);
            }
            if (portrait.gameObject.active == true)
            {
                portrait.gameObject.SetActive(false);
                
            }
            if(spriteName == "roboNeutral")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[0];
            }
            if (spriteName == "roboClosedEyes")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[1];
            }
            if (spriteName == "roboWhiteEyes")
            {
                //print("white eyes");
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[2];
            }
            if (spriteName == "roboBlackEyes")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[3];
            }
            if (spriteName == "roboScared")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[4];
            }
            if (spriteName == "roboSurprised")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[5];
            }
            if (spriteName == "roboConfused")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[6];
            }
            if (spriteName == "roboBlush")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[7];
            }
            if (spriteName == "roboHappy")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[8];
            }
            if (spriteName == "roboScaredEyesClosed")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[9];
            }
            if(spriteName == "roboDefiant")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[10];
            }
            if (spriteName == "roboScaredFists")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[11];
            }
            if (spriteName == "roboScaredYell")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[12];
            }
            if (spriteName == "roboScaredFistsYell")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[13];
            }
            if (spriteName == "roboWistful")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[19];
            }
            if (spriteName == "roboCalm")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[20];
            }
            if (spriteName == "eliseShock")
            {
                portrait2.gameObject.GetComponent<Image>().sprite = robogirl[16];
            }


            //portrait2.gameObject.GetComponent<Image>().SetNativeSize();
        }

        public void setElise(string spriteName)
        {
            if (cgbackground.gameObject.activeSelf == true)
            {
                cgbackground.gameObject.SetActive(false);

            }
            characterName.GetComponent<TMPro.TextMeshProUGUI>().text = "Elise";
            characterName.gameObject.SetActive(true);
            portrait3.gameObject.SetActive(true);
            portrait3.gameObject.GetComponent<Image>().enabled = true;
            portrait2.gameObject.SetActive(false);
            portrait.gameObject.SetActive(false);
            if (spriteName == "eliseDefiant")
            {
                portrait3.gameObject.GetComponent<Image>().sprite = robogirl[14];
            }
            if (spriteName == "eliseDisgust")
            {
                portrait3.gameObject.GetComponent<Image>().sprite = robogirl[15];
            }
            if (spriteName == "eliseShock")
            {
                portrait3.gameObject.GetComponent<Image>().sprite = robogirl[16];
            }
            if (spriteName == "eliseShockNoGrip")
            {
                portrait3.gameObject.GetComponent<Image>().sprite = robogirl[17];
            }
            if (spriteName == "eliseYell")
            {
                portrait3.gameObject.GetComponent<Image>().sprite = robogirl[18];
            }
        }
        public void setVex(string spriteName)
        {
            if(cgbackground.gameObject.activeSelf == true)
            {
                cgbackground.gameObject.SetActive(false);
                
            }
            characterName.GetComponent<TMPro.TextMeshProUGUI>().text = "Vex";
            characterName.gameObject.SetActive(true);
            portrait.gameObject.SetActive(true);
            portrait.gameObject.GetComponent<Image>().enabled = true;
            portrait2.gameObject.SetActive(false);
            portrait3.gameObject.SetActive(false);

            if (spriteName == "vexNeutral")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[0];
            }
            if (spriteName == "vexThinking")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[1];
            }
            if (spriteName == "vexSurprised")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[2];
            }
            if (spriteName == "vexTired")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[3];
            }
            if (spriteName == "vexGlasses")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[4];
            }
            if (spriteName == "vexConfused")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[5];
            }
            if (spriteName == "vexUnamused")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[6];
            }
            if (spriteName == "vexHappy")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[7];
            }
            if (spriteName == "vexContent")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[8];
            }
            if (spriteName == "vexCreepy")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[9];
            }
            if (spriteName == "vexDesperate")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[10];
            }
            if (spriteName == "vexGlassesCreepy")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[11];
            }
            if (spriteName == "vexGlassesSmile")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[12];
            }
            if (spriteName == "vexQuirky")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[13];
            }
            if (spriteName == "vexResigned")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[14];
            }
            if (spriteName == "vexShocked")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[15];
            }
            if (spriteName == "vexHorrified")
            {
                portrait.gameObject.GetComponent<Image>().sprite = vex[16];
            }
            //portrait.gameObject.GetComponent<Image>().SetNativeSize();
        }

        public void handleFade(int value)
        {
            if(!blackScreen.gameObject.activeSelf)
            {
                blackScreen.gameObject.SetActive(true);
            }
            Animator bsa = blackScreen.GetComponent<Animator>();
            bsa.speed = 1;
            if (value == 0) // fade to black
            {
                print("fading to black");
                bsa.Play("fadetoblack");

            }
            if(value == 1) //fade from black
            {
                bsa.Play("fadefromblack");
            }
            if(value == 2) //slowly fade to black
            {
                bsa.speed = 0.5f;
                bsa.Play("fadetoblack");
            }
            if(value == 3) //fade to black and show end screen
            {
                bsa.speed = 0.5f;
                bsa.Play("fadetoblack");
                StartCoroutine("showEndScreen");

            }
            else
            {
                bsa.Play("fadefromblack");
            }
            StartCoroutine("wait1sec");
        }

        

        public IEnumerator showEndScreen()
        {
            yield return new WaitForSeconds(2f);
            hideCG(12);
            endScreen.gameObject.SetActive(true);
            blackScreen.gameObject.SetActive(false);
        }

        public IEnumerator wait1sec()
        {
            yield return new WaitForSeconds(1f);
        }


        void noName(string x)
        {
            characterName.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }


        void setEveryone(string everyone)
        {
            characterName.GetComponent<TMPro.TextMeshProUGUI>().text = "Everyone";
            portrait.gameObject.GetComponent<Image>().enabled = false;
        }

        public void runMaintenance(int seg)
        {

            ma.activatePanel();
            //ma.maintenanceDialogue = (20 - ((6 - seg) * 4)) - 3;

            //print("dhmd is " + ma.maintenanceDialogue.ToString());
            ma.segment = seg;
            if(seg == 1)
            {
                //return;
            }
            else
            {
                if (seg % 2 == 1) //odd segments = closing panel, even segments = opening panel
                {
                    print("opening panel");
                    ma.prepPanel("open");
                    if(seg == 3)
                    {
                        ma.maintenanceDialogue = 4;
                    }
                    if (seg == 5)
                    {
                        ma.maintenanceDialogue = 12;
                    }

                }
                else
                {
                    print("closing panel");
                    ma.prepPanel("close");
                    if (seg == 2)
                    {
                        ma.maintenanceDialogue = 0;
                    }
                    if (seg == 4)
                    {
                        ma.maintenanceDialogue = 8;
                    }
                    if (seg == 6)
                    {
                        ma.maintenanceDialogue = 16;
                    }
                }
            }

        }

        public void runMaintenanceEndDay(int seg)
        {

        }
        public void maintenanceScrew()
        {
            if(ma.gameObject.active == true)
            {
                for (int i = 0; i < ma.screws.Length; i++)
                {
                    ma.screws[i].gameObject.GetComponent<Button>().interactable = true;
                }
            }

        }
        
        public void runNode(string nodeName)
        {
            StartDialogue(nodeName,"");
        }

        public void showSprites()
        {
            portrait.gameObject.SetActive(true);
            portrait2.gameObject.SetActive(true);
        }
        public void hideSprites(string x)
        {
            x = "";
            portrait.gameObject.SetActive(false);
            portrait2.gameObject.SetActive(false);
            portrait3.gameObject.SetActive(false);
        }

        public void cutscene1()
        {
            setVex("vex");
            StartDialogue("Cutscene1","");
        }

        public void StartDialogue(string convo, string name)
        {
            dialogueSystem.gameObject.SetActive(true);
            if(name == "vex")
            {
                portrait.gameObject.GetComponent<Image>().enabled = true;
                portrait.gameObject.SetActive(true);
            }
            if(name == "robo")
            {
                portrait2.gameObject.GetComponent<Image>().enabled = true;
                portrait2.gameObject.SetActive(true);
            }
            else
            {
                print("dialogue");
            }

                isDialogueActive = true;
            dr.StartDialogue(convo);
        }
        
        public void EndDialogue(string end)
        {
            dialogueSystem.gameObject.SetActive(false);
            portrait.gameObject.GetComponent<Image>().enabled = false;
            portrait2.gameObject.GetComponent<Image>().enabled = false;
            isDialogueActive = false;
        }
        
        public bool isActive()
        {
            return isDialogueActive;
        }
       
        public void allowObjectClicks()
        {
            print("allow");
            allowClicks = true;
        }
        public void disableClicks()
        {
            allowClicks = false;
        }

        public void pickChoice1(int choice)
        {
            if(choice == 1)
            {
                tereseEnding++;
            }
            if(choice == 2)
            {
                eliseEnding++;
            }
        }

        public void choice1(int num)
        {
            if(tereseEnding == 1)
            {
                StartDialogue("choice2a", "robo");
            }
            else
            {
                StartDialogue("choice2b", "vex");
            }
        }
    }


}
