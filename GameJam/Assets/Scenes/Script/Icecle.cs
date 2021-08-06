using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icecle : MonoBehaviour
{
    [Header("�L�����N�^�[�̃I�u�W�F�N�g��I������")]
    public GameObject Charcter;
    //�L�����N�^�[�̃|�W�V�������擾
    Vector3 CharPos;
    //���̍��W���擾
    Vector3 Ice;
    //�M�~�b�N���̂̍��W
    float IcePos;
    [Header("��炪��������͈�")]
    public float Range;
    [Header("��炪�����鑬�x")]
    public float YSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //�L�����N�^�[�̃I�u�W�F�N�g�̃R���|�[�l���g���擾����
        GameObject Charcter = GetComponent<GameObject>();
        //���̍��W���擾
        Ice = this.transform.position;
        IcePos = Ice.y;
    }

    // Update is called once per frame
    void Update()
    {
        //�L�����N�^�[�̍��W���擾����
        CharPos = Charcter.transform.position;
        //�L�����N�^�[�Ƃ��͈̔͂��m�F����
        if (-Range < (Ice.x - CharPos.x) && Range > (Ice.x - CharPos.x))
        {
            IcePos -= YSpeed;
            transform.position = new Vector2(Ice.x, IcePos);
        }
    }
}
