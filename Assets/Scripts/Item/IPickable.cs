using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    public void Picked(PlayerController player);
    public void Dropped();
}
