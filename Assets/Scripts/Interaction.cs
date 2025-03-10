using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;         //�󸶳� ���� ray�� ���� üũ�� ������ (0.05�ʸ��� üũ)
    private float lastCheckTime;            //���������� üũ�� �ð�
    public float maxCheckDistance = 10f;          //�󸶳� �ָ� �ִ� ���� üũ�� ������ (�����Ÿ�)
    public LayerMask layerMask;             //���̾� ����ũ (� ���̾ üũ�� ������)

    public GameObject interactObject;       //���� ��ȣ�ۿ� �ϰ��ִ� ���� ������Ʈ
    private IInteractable interactable;     //���� ��ȣ�ۿ� �ϰ��ִ� �������̽�

    public TextMeshProUGUI promptText;      //��ȣ�ۿ� ������Ʈ�� ������ ����� TextMeshProUGUI

    public Vector3 correctionVector;

    private void OnValidate()
    {
        layerMask = LayerMask.GetMask("Interactable");
        correctionVector = this.GetComponent<CapsuleCollider>().center;
    }

    private void Start()
    {
        //�Ҵ��� ���� �ߴٸ� Find�� ã�Ƽ� �־��ش� (����ڵ�)
        if (promptText == null)
            promptText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            //ray�� ĳ���� �������� ������...
            Ray ray = new Ray(transform.position + correctionVector, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            //ray�� ���� ������Ʈ ������ ������ ����
            RaycastHit hit;

            Debug.Log("���");

            //maxCheckDistance �Ÿ� ������ layerMask�� �ش��ϴ� ������Ʈ�� ����
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                Debug.Log(hit.collider.gameObject.name);

                //���� ���ο� ������Ʈ�� �����ߴٸ�
                if (hit.collider.gameObject != interactObject)
                {
                    //������ ���� ������Ʈ ����
                    interactObject = hit.collider.gameObject;
                    //������ ���� ������Ʈ�� IInteractable �������̽� ��������
                    interactable = hit.collider.GetComponent<IInteractable>();

                    //������Ʈ�� ���
                    promptText.gameObject.SetActive(true);
                    promptText.text = interactable.GetinteractText();
                }
            }
            else
            {
                //���� ������ ������Ʈ�� ���ٸ�
                interactObject = null;
                interactable = null;

                //������Ʈ(UI) �����
                promptText.gameObject.SetActive(false);
            }
        }
    }

    //�ݱ� �޼��尡 �� �ڸ�

}


