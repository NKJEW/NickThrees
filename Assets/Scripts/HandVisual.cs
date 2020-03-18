using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandVisual : MonoBehaviour
{
	public abstract void Init(List<CardData> hand, List<CardData> visible, List<CardData> hidden);
}
