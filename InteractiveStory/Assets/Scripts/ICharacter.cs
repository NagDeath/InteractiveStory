using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void SetupCharacter(Passage block);
    void DestroyCharacter();
}
