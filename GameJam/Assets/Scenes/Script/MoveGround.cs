using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [Header("�X�^�[�g�̃Q�[���I�u�W�F�N�g")]
    public GameObject start;
    [Header("�S�[���̃Q�[���I�u�W�F�N�g")]
    public GameObject end;
    [Header("�ړ��ʂ̒���")]
    public float MoveSpeed;
    //x���W�̈ړ���
    float Movex;
    //y���W�̈ړ���
    float Movey;
    //x���W�̃|�W�V����
    float Posx;
    //y���W�̃|�W�V����
    float Posy;
    //�X�^�[�g�̃I�u�W�F�N�g�̍��W���擾����
    Vector3 PosS;
    //�G���h�̃I�u�W�F�N�g�̍��W���擾����
    Vector3 PosE;
    //����������

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���I�u�W�F�N�g�̃R���|�[�l���g���擾����
        GameObject start = GetComponent<GameObject>();
        GameObject end = GetComponent<GameObject>();
        //�擾�����I�u�W�F�N�g�̍��W���擾
        PosS = start.transform.position;
        PosE = end.transform.position;
        //�ړ��ʂ��Z�o����
        Movex = (PosE.x - PosS.x) / MoveSpeed;
        Movey = (PosE.y - PosS.y) / MoveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //���W�̃|�W�V���������Z����
        Posx += Movex;
        Posy += Movey;
        //�擾�����I�u�W�F�N�g�͈̔͂��z�����ꍇ�̏�����
        if(PosS.x - Posx >= 0 && PosS.y - Posy >= 0)
        {
            //�ړ��ʂ𔽓]����
            Movex = -Movex;
            Movey = -Movey;
        }
        else if(PosE.x - Posx >= 0 && PosE.y - Posy >= 0)
        {
            //�ړ��ʂ𔽓]����
            Movex = -Movex;
            Movey = -Movey;
        }
        //���W��ύX����
        transform.position = new Vector2(Posx, Posy);
    }
}
