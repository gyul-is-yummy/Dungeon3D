using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;         //얼마나 자주 ray를 쏴서 체크할 것인지 (0.05초마다 체크)
    private float lastCheckTime;            //마지막으로 체크한 시간
    public float maxCheckDistance = 10f;          //얼마나 멀리 있는 것을 체크할 것인지 (사정거리)
    public LayerMask layerMask;             //레이어 마스크 (어떤 레이어를 체크할 것인지)

    public GameObject interactObject;       //현재 상호작용 하고있는 게임 오브젝트
    private IInteractable interactable;     //현재 상호작용 하고있는 인터페이스

    public TextMeshProUGUI promptText;      //상호작용 오브젝트의 정보를 띄워줄 TextMeshProUGUI

    public Vector3 correctionVector;

    private void OnValidate()
    {
        layerMask = LayerMask.GetMask("Interactable");
        correctionVector = this.GetComponent<CapsuleCollider>().center;
    }

    private void Start()
    {
        //할당을 깜빡 했다면 Find로 찾아서 넣어준다 (방어코드)
        if (promptText == null)
            promptText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            //ray를 캐릭터 기준으로 쏴야함...
            Ray ray = new Ray(transform.position + correctionVector, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            //ray를 맞은 오브젝트 정보를 저장할 변수
            RaycastHit hit;

            Debug.Log("쏜다");

            //maxCheckDistance 거리 내에서 layerMask에 해당하는 오브젝트만 감지
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                Debug.Log(hit.collider.gameObject.name);

                //만약 새로운 오브젝트를 감지했다면
                if (hit.collider.gameObject != interactObject)
                {
                    //감지된 게임 오브젝트 저장
                    interactObject = hit.collider.gameObject;
                    //감지된 게임 오브젝트의 IInteractable 인터페이스 가져오기
                    interactable = hit.collider.GetComponent<IInteractable>();

                    //프롬포트에 출력
                    promptText.gameObject.SetActive(true);
                    promptText.text = interactable.GetinteractText();
                }
            }
            else
            {
                //만약 감지된 오브젝트가 없다면
                interactObject = null;
                interactable = null;

                //프롬포트(UI) 숨기기
                promptText.gameObject.SetActive(false);
            }
        }
    }

    //줍기 메서드가 들어갈 자리

}


