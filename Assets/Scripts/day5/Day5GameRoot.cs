using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��Ϸ���
/// </summary>
public class Day5GameRoot : MonoBehaviour
{
    public static Day5GameRoot ins;
    /// <summary>
    /// ��¼ʵ�������
    /// </summary>
    public EntityManager em;
    /// <summary>
    /// ��¼��קʵ��
    /// </summary>
    public Entity dragEntity;
    /// <summary>
    /// ��¼ʵ�����������
    /// </summary>
    public Day5InsData data;
    /// <summary>
    /// ��¼ ��ק��ui������
    /// </summary>
    public string dragUIName;
    private void Awake()
    {
        if (ins == null) { ins = this; }
    }
}
