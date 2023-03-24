using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ConversationModel;

public class LoadDataConversation : MonoBehaviour
{
    public TextAsset textJsonConversation;
    public List<Sprite> sprites;

    private List<Main> _mainConversation;
    private int CounterMainProgress = 0;
    private bool MainAnswerA = false;


    private bool EndingDenialFlag = false;
    private List<EndingDenial> _endingDenialConversation;
    private int CounterEndingProgress = 0;

    private bool InformationStatusFlag = false;
    private List<InformationStatus> _informationStatusConversation;
    private int CounterStatusProgress = 0;

    private bool InformationTalentFlag = false;
    private List<InformationTalent> _informationTalentConversation;
    private int CounterTalentProgress = 0;

    private bool InformationBagFlag = false;
    private List<InformationBag> _informationBagConversation;
    private int CounterBagProgress = 0;

    private bool InformationMapFlag = false;
    private List<InformationMap> _informationMapConversation;
    private int CounterMapProgress = 0;

    private bool InformationQuestFlag = false;
    private List<InformationQuest> _informationQuestConversation;
    private int CounterQuestProgress = 0;

    private bool InformationBattleFlag = false;
    private List<InformationBattle> _informationBattleConversation;
    private int CounterBattleProgress = 0;

    private bool TrueEndingFlag = false;
    private List<EndingTrue> _endingTrueConversation;
    private int CounterTrueEndingProgress = 0;

    void Start()
    {
        ConversationModel cm = new ConversationModel(textJsonConversation);
        this._mainConversation = cm.ReaderData().Conversation.Main;

        this._endingDenialConversation = cm.ReaderData().Conversation.EndingDenial;

        this._endingTrueConversation = cm.ReaderData().Conversation.EndingTrue;

        this._informationStatusConversation = cm.ReaderData().Conversation.InformationStatus;

        this._informationTalentConversation = cm.ReaderData().Conversation.InformationTalent;

        this._informationBagConversation = cm.ReaderData().Conversation.InformationBag;

        this._informationMapConversation = cm.ReaderData().Conversation.InformationMap;

        this._informationQuestConversation = cm.ReaderData().Conversation.InformationQuest;

        this._informationBattleConversation = cm.ReaderData().Conversation.InformationBattle;
    }

    void Update()
    {
        if (EndingDenialFlag == true)
        {
            LoadEndingDenial();    
        }
        else if(InformationStatusFlag == true)
        {
            LoadConversationStatus();
        }
        else if(InformationTalentFlag == true)
        {
            LoadConversationTalent();
        }
        else if(InformationBagFlag == true)
        {
            LoadConversationBag();
        }
        else if(InformationQuestFlag == true)
        {
            LoadConversationQuest();
        }
        else if(InformationMapFlag == true)
        {
            LoadConversationMap();
        }
        else if(InformationBattleFlag == true)
        {
            LoadConversationBattle();
        }
        else if(AbstractPlayerData.player != null && AbstractPlayerData.GetCurrentMainQuest() == null)
        {
            LoadTrueEnding();
        }

        else 
        {
            LoadCanvas();     
        }
    }

    void LoadCanvas()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        GameObject character = GameObject.Find("User Interface").transform.GetChild(1).gameObject;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        if(_mainConversation[CounterMainProgress].Source == "")
        {
            character.SetActive(false);
        }
        else
        {
            character.SetActive(true);
        }

        foreach(Sprite sprite in sprites)
        {
            if (_mainConversation[CounterMainProgress].Source == sprite.name)
            {
                character.GetComponent<Image>().sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _mainConversation[CounterMainProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        if(_mainConversation[CounterMainProgress].Option.Count > 0)
        {
            child_choiceWrapper.SetActive(true);

            Button child_choiceWrapper_buttonA = child_choiceWrapper.transform.GetChild(0).gameObject.GetComponent<Button>() == null? child_choiceWrapper.transform.GetChild(0).gameObject.AddComponent<Button>() as Button : child_choiceWrapper.transform.GetChild(0).gameObject.GetComponent<Button>() as Button;
            Button child_choiceWrapper_buttonB = child_choiceWrapper.transform.GetChild(1).gameObject.GetComponent<Button>() == null? child_choiceWrapper.transform.GetChild(1).gameObject.AddComponent<Button>() as Button : child_choiceWrapper.transform.GetChild(1).gameObject.GetComponent<Button>() as Button;
            Text child_choiceWrapper_textA = child_choiceWrapper.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
            Text child_choiceWrapper_textB = child_choiceWrapper.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;

            child_choiceWrapper_textA.text = _mainConversation[CounterMainProgress].Option[0].Response;
            child_choiceWrapper_textB.text = _mainConversation[CounterMainProgress].Option[1].Response;

            child_choiceWrapper_buttonA.onClick.RemoveAllListeners();
            child_choiceWrapper_buttonA.onClick.AddListener(() => {
                if(_mainConversation[CounterMainProgress].Part == "Information")
                {
                    GameObject.Find("User Interface").transform.GetChild(3).gameObject.SetActive(true);
                }else
                {
                    this.CounterMainProgress += 1;
                }
                
            });

            child_choiceWrapper_buttonB.onClick.RemoveAllListeners();
            child_choiceWrapper_buttonB.onClick.AddListener(() => {
                if(_mainConversation[CounterMainProgress].Part == "Information")
                {
                    this.CounterMainProgress += 1;
                }
                else
                {
                    if(_mainConversation[CounterMainProgress].Option[1].IsEnding == false)
                    {   
                        this.CounterMainProgress += 2;
                    }
                    else
                    {
                        this.EndingDenialFlag = true;
                    }
                }
            });
        }
        else
        {
            child_nextWrapper.SetActive(true);
            Button child_nextWrapper_button;
            if(child_nextWrapper.GetComponent<Button>() == null)
            {
                child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
            }
            else
            {
                child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
            }
            child_nextWrapper_button.onClick.RemoveAllListeners();
            child_nextWrapper_button.onClick.AddListener(() => {
                this.CounterMainProgress += _mainConversation[CounterMainProgress - 1 < 0? 0 : CounterMainProgress].Part == "Choice-A"? 2 : 1;
                if(this.CounterMainProgress > _mainConversation.Count - 1)
                {
                    GameObject.Find("Initialize").GetComponent<SceneChanger>().InGameScene();
                }
            });
        }
        

    }

    void LoadEndingDenial()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_endingDenialConversation[CounterEndingProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _endingDenialConversation[CounterEndingProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {

            this.CounterEndingProgress += 1;
            if(this.CounterEndingProgress > _endingDenialConversation.Count - 1)
            {
                GameObject.Find("Initialize").GetComponent<SceneChanger>().ExitMainMenu();
            }
        });  
    }

    void LoadTrueEnding()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_endingTrueConversation[CounterTrueEndingProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = "Fotina";
        child_sentence.text = _endingTrueConversation[CounterTrueEndingProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {

            this.CounterTrueEndingProgress += 1;
            if(this.CounterTrueEndingProgress > _endingTrueConversation.Count - 1)
            {
                GameObject.Find("Initialize").GetComponent<SceneChanger>().CreditScene();
            }
        });  
    }

    public void OnClickInformation(string informationName)
    {
        GameObject.Find("User Interface").transform.GetChild(3).gameObject.SetActive(false);
        if(informationName == "Status")
        {
            this.InformationStatusFlag = true;
        }
        else if (informationName == "Talent")
        {
            this.InformationTalentFlag = true;
        }
        else if (informationName == "Bag")
        {
            this.InformationBagFlag = true;
        }
        else if (informationName == "Map")
        {
            this.InformationMapFlag = true;
        }
        else if (informationName == "Quest")
        {
            this.InformationQuestFlag = true;
        }
        else if (informationName == "Battle")
        {
            this.InformationBattleFlag = true;
        }
    }

    void LoadConversationStatus()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_informationStatusConversation[CounterStatusProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _informationStatusConversation[CounterStatusProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {

            this.CounterStatusProgress += 1;
            if (this.CounterStatusProgress >= _informationStatusConversation.Count)
            {
                this.CounterStatusProgress = 0;
                this.InformationStatusFlag = false;
            }
        });  
    }

    void LoadConversationTalent()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_informationTalentConversation[CounterTalentProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _informationTalentConversation[CounterTalentProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {
            this.CounterTalentProgress += 1;
            if (this.CounterTalentProgress >= _informationTalentConversation.Count)
            {
                this.CounterTalentProgress = 0;
                this.InformationTalentFlag = false;
            }
        });  
    }

    void LoadConversationBag()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_informationBagConversation[CounterBagProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _informationBagConversation[CounterBagProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {
            this.CounterBagProgress += 1;
            if (this.CounterBagProgress >= _informationBagConversation.Count)
            {
                this.CounterBagProgress = 0;
                this.InformationBagFlag = false;
            }
        });  
    }

    void LoadConversationMap()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_informationMapConversation[CounterMapProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _informationMapConversation[CounterMapProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {
            this.CounterMapProgress += 1;
            if (this.CounterMapProgress >= _informationMapConversation.Count)
            {
                this.CounterMapProgress = 0;
                this.InformationMapFlag = false;
            }
        });  
    }

    void LoadConversationQuest()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_informationQuestConversation[CounterQuestProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _informationQuestConversation[CounterQuestProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {
            this.CounterQuestProgress += 1;
            if (this.CounterQuestProgress >= _informationQuestConversation.Count)
            {
                this.CounterQuestProgress = 0;
                this.InformationQuestFlag = false;
            }
        });  
    }

    void LoadConversationBattle()
    {
        GameObject parent = GameObject.Find("[Box] Conversation");

        Image character = GameObject.Find("Character").GetComponent<Image>() as Image;
        Text child_name = parent.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_sentence = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject child_nextWrapper = parent.transform.GetChild(3).gameObject;
        GameObject child_choiceWrapper = parent.transform.GetChild(4).gameObject;

        foreach(Sprite sprite in sprites)
        {
            if (_informationBattleConversation[CounterBattleProgress].Source == sprite.name)
            {
                character.sprite = sprite;
            }
        }

        child_name.text = this.CounterMainProgress > 5? "Fotina" : "Unknown";
        child_sentence.text = _informationBattleConversation[CounterBattleProgress].Sentence;

        child_nextWrapper.SetActive(false);
        child_choiceWrapper.SetActive(false);

        
        child_nextWrapper.SetActive(true);
        Button child_nextWrapper_button;
        if(child_nextWrapper.GetComponent<Button>() == null)
        {
            child_nextWrapper_button = child_nextWrapper.AddComponent<Button>() as Button;
        }
        else
        {
            child_nextWrapper_button = child_nextWrapper.GetComponent<Button>() as Button;
        }
        child_nextWrapper_button.onClick.RemoveAllListeners();
        child_nextWrapper_button.onClick.AddListener(() => {
            this.CounterBattleProgress += 1;
            if (this.CounterBattleProgress >= _informationBattleConversation.Count)
            {
                this.CounterBattleProgress = 0;
                this.InformationBattleFlag = false;
            }
        });  
    }

    public void SkipButton()
    {
        if(TrueEndingFlag)
        {   
            GameObject.Find("Initialize").GetComponent<SceneChanger>().CreditScene();
        }
        else
        {
            GameObject.Find("Initialize").GetComponent<SceneChanger>().InGameScene();
        }
    }
}
