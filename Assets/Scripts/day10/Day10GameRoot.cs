using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ϸ���
/// </summary>
public class Day10GameRoot : MonoBehaviour
{
    public static Day10GameRoot ins;
    private void Awake()
    {
       if (ins == null)
        {
            ins = this;
        }
    }
    /// <summary>
    /// ��¼ʵ�����
    /// </summary>
    public EntityManager em;
    /// <summary>
    /// ��¼��ק��ʵ��
    /// </summary>
    public Entity dragEntity;
    
    /// <summary>
    /// ��¼ʵ�����������
    /// </summary>
    public Day10InsData insData;

    /// <summary>
    /// ��קui������
    /// </summary>
    public string dragUIName;
    public Text diamondDes;
    public int dCount;

    public Button btn_CreateSoldier;
    private Vector3 bPos;//��¼��������λ��
    private void Start()
    {
        btn_CreateSoldier.onClick.AddListener(OnCreateSoldier);

        size.onEndEdit.AddListener(OnSetSize);
        InitGrid(10);
    }

    private void OnCreateSoldier()
    {
        btn_CreateSoldier.gameObject.SetActive(false);
        //����һ��ʿ��
        var sEntity = em.Instantiate(insData.soldier);
        //���λ��
        var rndPos = UnityEngine.Random.insideUnitCircle * 5;
        var realPos = bPos + new Vector3(rndPos.x,0 ,rndPos.y);
        em.SetComponentData(sEntity, new LocalTransform {  Position= realPos, Scale=1});
        em.AddComponentData(sEntity, new Day10SoldierData { targetPos=realPos, hp=100});
        AddHpToEntity(em, sEntity,realPos);
    }


    public void AddHpToEntity(EntityManager em,Entity entity,Vector3 pos)
    {
        Transform sliTran = Instantiate(Resources.Load<GameObject>("hp"),transform).transform;
        sliTran.position = Camera.main.WorldToScreenPoint(pos+Vector3.up);
        var sli=sliTran.GetComponent<Slider>();
        em.AddComponentObject(entity, new Day10Hp {  sli=sli});
    }
    /// <summary>
    /// �Ƿ���ʾUI
    /// </summary>
    public void ShowSoldierUI(Vector3 pos)
    {
        btn_CreateSoldier.gameObject.SetActive(true);
        btn_CreateSoldier.transform.position = Camera.main.WorldToScreenPoint(pos+Vector3.left*3);
        bPos = pos;
    }


    public InputField size;
    public Transform gridParent;
  

    private void OnSetSize(string arg0)
    {
        if (!string.IsNullOrEmpty(arg0))
        {
            int size = int.Parse(arg0);
            InitGrid(size);
        }
    }

    public Collider[] GetSize(Vector3 pos, float size)
    {
        Vector3 half = new Vector3(size, size, size);
        return Physics.OverlapBox(pos, half / 2);
    }


    public  bool HasOwn(Collider[] col)
    {
        bool has = false;
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].name=="1")
            {
                has = true;
                break;
            }
        }
        return has;
    } 
    private void InitGrid(int size)
    {
        DestroyAllChild(gridParent);
        for (int i = 0; i < size; i++)
        {
            for (global::System.Int32 j = 0; j < size; j++)
            {
                Transform tran = Instantiate(Resources.Load<GameObject>("cell"), gridParent).transform;
                tran.position = new Vector3(i, 0, j);
                tran.name = "0";
            }
        }
    }

    private void DestroyAllChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
