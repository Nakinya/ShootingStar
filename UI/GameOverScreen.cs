using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] AudioData confirmGameOverSound;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");
    Canvas canvas;
    Animator animator;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }
    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        playerInput.onConfirmGameOver += OnConfirmGameOver;
    }
    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        playerInput.onConfirmGameOver -= OnConfirmGameOver;

    }
    void OnGameOver()
    {
        hUDCanvas.enabled = false;//�ر�HUD
        canvas.enabled = true;//�򿪽�������UI
        animator.enabled = true;//���Ŷ���
        playerInput.DisableAllInputs();//������������
    }
    //Animation Event
    void EnableGameOverScreenInput()
    {
        playerInput.EnableGameOverScreenInput();
    }
    void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        playerInput.DisableAllInputs();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene();//���ػ��ֻ���
    }
}
