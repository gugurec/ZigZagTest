using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//не знаю где тут можно применить интерфейсы, но добавить нужно, по этому сделал простой интерфейс,
//который бы собирал информацию об объекте (для дебага например)
public interface IStateInfo
{
    string GetInfo();
}
