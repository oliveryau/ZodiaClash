using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Others")]
    public GameObject joinIndicator;
    [SerializeField] private SceneTransition sceneTransition;

    public bool IsOpen { get; private set; }

    private DialogueTypewriter dialogueTypewriter;
    private ScenesManager scenesManager;

    private void Start()
    {
        dialogueTypewriter = GetComponent<DialogueTypewriter>();
        scenesManager = FindObjectOfType<ScenesManager>();

        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (DialogueLine dialogueLine in dialogueObject.DialogueLines)
        {
            speakerText.text = dialogueLine.speaker;
            yield return RunTypingEffect(dialogueLine.dialogue);
            dialogueText.text = dialogueLine.dialogue;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0));

            speakerText.text = null;
        }

        CloseDialogueBox(dialogueObject.id);
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        dialogueTypewriter.Run(dialogue, dialogueText);

        while (dialogueTypewriter.isRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
            {
                dialogueTypewriter.Stop();
            }
        }
    }

    private void CloseDialogueBox(int id = 0)
    {
        dialogueBox.SetActive(false);
        dialogueText.text = null;

        switch (id)
        {
            case 2:
            case 3:
            case 4:

                sceneTransition.camPrevPosition = Camera.main.transform.position;
                sceneTransition.prevPosition = GameObject.FindWithTag("Player").transform.position;
                StartCoroutine(scenesManager.LoadLevelFromMap(id));
                StartCoroutine(CloseDialogueDelay());
                break;
            case 5:

                sceneTransition.defeatedEnemyNpcs.Add("Temp Ox");
                _ExplorationManager explorationManager = FindObjectOfType<_ExplorationManager>();
                foreach (string enemyName in sceneTransition.defeatedEnemyNpcs)
                {
                    foreach (GameObject enemy in explorationManager.enemyNpcs)
                    {
                        if (enemy.name == "Temp Ox")
                        {
                            enemy.SetActive(false); //hide defeated enemy npcs
                            break;
                        }
                    }
                }
                joinIndicator.GetComponentInChildren<TextMeshProUGUI>().text = "Leishou has joined your team.";
                StartCoroutine(StepThroughJoinIndicatorDelay());
                break;
            case 93:

                joinIndicator.GetComponentInChildren<TextMeshProUGUI>().text = "Yangsheng has joined your team.";
                StartCoroutine(StepThroughJoinIndicatorDelay());
                break;
            case 0:
            default:

                IsOpen = false;
                break;
        }
    }

    public IEnumerator StepThroughJoinIndicatorDelay()
    {
        joinIndicator.SetActive(true);

        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0));

        joinIndicator.SetActive(false);
        joinIndicator.GetComponentInChildren<TextMeshProUGUI>().text = null;
        IsOpen = false;
    }

    private IEnumerator CloseDialogueDelay()
    {
        yield return new WaitForSeconds(1f);

        IsOpen = false;
    }
}
