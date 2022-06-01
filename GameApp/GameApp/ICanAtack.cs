using System;

interface ICanAtack : IMoveable
{
    bool IsAtacking { get; set; }

    void Atack();
}
