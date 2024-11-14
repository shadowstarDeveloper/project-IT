using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemClass : MonoBehaviour
{
    
    
    public string Tag; //고유이름
    public string spriteTag;
    public string miscType; //wood, metal, meat, vegitable, ... 
    public int num, maxNum; //갯수,최대갯수 
    public int[,] matrix; //가로 세로 크기
    public float weight;
    public float amount; //기본값 0, 무게는 weight+amount ; 물병 0.2kg + 1L = 1.2 kg

}

public class food : itemClass
{
    //음식
    public float decay; //-1 썩지 않음, 신선도 시간 0~ hr
    public float maxDecay; //최대 신선도 시간 
    public float kcal; //열량
}
public class liqidContainer: itemClass, I_fluid
{
    //리퀴드는 물이나 음료수, 기름통과 같이 액체를 담는 컨테이너
    public string containType { get; set; } 
    public float maxFluid { get; set; }
}
public class solidContainer : itemClass
{
    //가방류
    public int[,] containerMatrix; //아이템을 담을 인벤토리 칸
    public List<itemClass> list; //아이템을 담은 리스트, 무게 계산 등 
    ///과연 itemClass로 담는 게 괜찮을 것인가?
}
public class tool : itemClass
{
    //도구
    public float duration; //내구도
    public float maxDuration; //최대내구도
    public string toolType; //axe, crowbar, blunt, hammer, driver, 
}
public class meleeWeapon : tool
{
    //도구를 상속받음
    public float damage;//대미지
}
public class rangeWeapon : tool
{
    public int round; //총알갯수, -1이면 없음?

}

////////인터페이스
public interface I_fluid
{
    public string containType { get; set; } //담은 액체류의 이름 water, juice, cola, oil, gasoil, acid ... 
    public float maxFluid { get; set; }//최대 액체량, 없으면 연산하지 않음
    
}

