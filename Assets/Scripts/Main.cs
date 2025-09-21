using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// - make animations work so that we can input idle animations and action
//   animations
// - ponder code logic :D (observation pattern)
// - make animation sprite sheets for everythinggggggg
// - implement start over (gg) button
// - get rid of go (unlucky), recenter buttons, add gg button to top by removing
//   resource bars once slime dies

public class Main : MonoBehaviour
{
    // Buttons
    public Button feedButton;
    public Button sleepButton;
    public Button petButton;
    public Button huntButton;

    // Slime State Textures
    public Image currentSlimeImg;
    public Image baseSlime;
    public Image boredSlime;
    public Image hungrySlime;
    public Image eatingSlime;
    public Image sadSlime;
    public Image sleepingSlime;
    public Image tiredSlime;
    public Image deadSlime;
    public Image shrunkSlime;

    // Resource Bars
    public Image currentHunger;
    public Image currentEnergy;
    public Image currentHappiness;

    // Button Actions (delegates)
    private UnityAction feedAction;
    private UnityAction sleepAction;
    private UnityAction petAction;
    private UnityAction huntAction;

    // Slime Stats
    private float hunger = 100f;
    private float energy = 100f;
    private float happiness = 100f;
    private float max = 100f;

    // Button Variables
    private bool buttonCooldown = false;
    private float buttonTimer = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set all slime textures to false besides base
        hungrySlime.gameObject.SetActive(false);
        boredSlime.gameObject.SetActive(false);
        eatingSlime.gameObject.SetActive(false);
        sadSlime.gameObject.SetActive(false);
        sleepingSlime.gameObject.SetActive(false);
        tiredSlime.gameObject.SetActive(false);
        deadSlime.gameObject.SetActive(false);
        shrunkSlime.gameObject.SetActive(false);

        currentSlimeImg = baseSlime;
        baseSlime.gameObject.SetActive(true);

        // add appropriate functions to unity actions
        feedAction += Feed;
        feedButton.onClick.AddListener(feedAction);

        sleepAction += Sleep;
        sleepButton.onClick.AddListener(sleepAction);

        petAction += Pet;
        petButton.onClick.AddListener(petAction);

        huntAction += HuntLog;
        huntButton.onClick.AddListener(huntAction);
    }

    // Update is called once per frame
    void Update()
    {
        // decreasing resource stats, prevents stats from going below 0
        hunger -= 5f * Time.deltaTime;
        if (hunger < 0f)
        {
            hunger = 0f;
        }

        happiness -= 5f * Time.deltaTime;
        if (happiness < 0f)
        {
            happiness = 0f;
        }

        energy -= 5f * Time.deltaTime;
        if (energy < 0f)
        {
            energy = 0f;
        }

        // checks button cooldown to see if they can be used
        if (buttonCooldown)
        {
            buttonTimer -= Time.deltaTime;

            // resets all buttons once cooldown goes to 0
            if (buttonTimer <= 0f)
            {
                feedButton.interactable = true;
                sleepButton.interactable = true;
                petButton.interactable = true;
                huntButton.interactable = true;

                buttonTimer = 0f;
                buttonCooldown = false;
            }
        }

        // updates resource bars
        UpdateHunger();
        UpdateHappiness();
        UpdateEnergy();
        UpdateSlimeState();
    }

    // updates slime texture according to its stats
    private void UpdateSlimeState()
    {
        Image newSlimeState = null;

        if (hunger <= 50f)
        {
            newSlimeState = hungrySlime;
        }
        else if (energy <= 50)
        {
            newSlimeState = tiredSlime;
        }
        else if (happiness <= 50)
        {
            newSlimeState = sadSlime;
        }
        else
        {
            newSlimeState = baseSlime;
        }

        if (currentSlimeImg != newSlimeState)
        {
            if (currentSlimeImg != null)
            {
                currentSlimeImg.gameObject.SetActive(false);
            }

            newSlimeState.gameObject.SetActive(true);
            currentSlimeImg = newSlimeState;
        }
    }

    // changes resource bars according to their stats
    private void UpdateHunger()
    {
        float ratio = hunger / max;
        currentHunger.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    private void UpdateHappiness()
    {
        float ratio = happiness / max;
        currentHappiness.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    private void UpdateEnergy()
    {
        float ratio = energy / max;
        currentEnergy.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    // your pet died :(
    private void GameOver() { }

    // functions run when buttons are pressed
    // increases hunger stat
    private void Feed()
    {
        hunger += 30;
        if (hunger > 100)
        {
            hunger = 100;
        }

        disableButtons();

        // StartCoroutine(SlimeAnimation(1));
    }

    // increases happiness stat
    private void Pet()
    {
        happiness += 20;
        if (happiness > 100)
        {
            happiness = 100;
        }

        disableButtons();

        // StartCoroutine(SlimeAnimation(2));
    }

    // increases energy stat
    private void Sleep()
    {
        energy += 50;
        if (energy > 100)
        {
            energy = 100;
        }

        disableButtons();

        // StartCoroutine(SlimeAnimation(3));
    }

    // temporary, prints hunt when go is pressed, go button behavior not set
    private void HuntLog()
    {
        Debug.Log("hunt");
    }

    // disables all buttons when one is pressed, starts button cooldown
    private void disableButtons()
    {
        feedButton.interactable = false;
        sleepButton.interactable = false;
        petButton.interactable = false;
        huntButton.interactable = false;

        buttonCooldown = true;
    }

    // what the fuck is this
    // probably not using this just use animator (wat is that lol)
    // private System.Collections.IEnumerator SlimeAnimation(int i)
    // {
    //     if (baseSlime.enabled)
    //     {
    //         eatingSlime.gameObject.SetActive(false);
    //     }
    //     else
    //         eatingSlime.gameObject.SetActive(true);
    //     baseSlime.gameObject.SetActive(false);

    //     yield return new WaitForSecondsRealtime(1);

    //     eatingSlime.gameObject.SetActive(false);
    //     baseSlime.gameObject.SetActive(true);
    // }
}
