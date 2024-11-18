using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemClass : MonoBehaviour
{
    
    
    public string Tag; //�����̸�
    public string spriteTag;
    public string miscType; //wood, metal, meat, vegitable, ... 
    public int num, maxNum; //����,�ִ밹�� 
    public int row, col; // ������ ũ��
    public float weight;
    public float amount; //�⺻�� 0, ���Դ� weight+amount ; ���� 0.2kg + 1L = 1.2 kg

}

public class food : itemClass
{
    //����
    public float decay; //-1 ���� ����, �ż��� �ð� 0~ hr
    public float maxDecay; //�ִ� �ż��� �ð� 
    public float kcal; //����
}
public class liqidContainer: itemClass, I_fluid
{
    //������� ���̳� �����, �⸧��� ���� ��ü�� ��� �����̳�
    public string containType { get; set; } 
    public float maxFluid { get; set; }
}
public class solidContainer : itemClass
{
    //�����
    public int[,] containerMatrix; //�������� ���� �κ��丮 ĭ
    public List<itemClass> list; //�������� ���� ����Ʈ, ���� ��� �� 
    ///���� itemClass�� ��� �� ������ ���ΰ�?
}
public class tool : itemClass
{
    //����
    public float duration; //������
    public float maxDuration; //�ִ볻����
    public string toolType; //axe, crowbar, blunt, hammer, driver, 
}
public class meleeWeapon : tool
{
    //������ ��ӹ���
    public float damage;//�����
}
public class rangeWeapon : tool
{
    public int round; //�Ѿ˰���, -1�̸� ����?

}

////////�������̽�
public interface I_fluid
{
    public string containType { get; set; } //���� ��ü���� �̸� water, juice, cola, oil, gasoil, acid ... 
    public float maxFluid { get; set; }//�ִ� ��ü��, ������ �������� ����
    
}

