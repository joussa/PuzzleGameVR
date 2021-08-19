using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateExamples : MonoBehaviour {

    // Delegate definition
    public delegate void SimpleAttack(int amount);

    // Fire attack
    public void FireAttack(int amount)
    {
        // show a fire animation..
        print("Fire Attack by " + amount);
    }

    // Ice Attack
    public void IceAttack(int amount)
    {
        // show a freezing effect
        print("Ice Attack by " + amount);
    }

    // copy the attack of your enemy
    public void CopyAttack(int amount, SimpleAttack EnemysAttack)
    {
        // do the same attack as your enemy
        EnemysAttack(amount);

        // the same as: EnemysAttack.Invoke(amount);
    }


    void Awake () {

        print("Example 1: Assign a method and execute it");

        SimpleAttack PlayerAttack = FireAttack;
        PlayerAttack(100); //execute fire attack


        print("Example 2: Pass a method as parameter");

        CopyAttack(200, IceAttack); //execute ice attack


        print("Example 3: Assign an method, execute, then assign a different method");

        SimpleAttack SomeAttack = FireAttack;
        SomeAttack(300); //execute fire attack
        SomeAttack = IceAttack;
        SomeAttack(301); //execute ice attack


        print("Example 4: Combine attacks, execute, then remove one and execute");
        SimpleAttack ManyAttacks = FireAttack;

        //combine (behind the scenes it's calling Delegate.Combine)
        ManyAttacks += IceAttack;

        ManyAttacks(400); //execute BOTH Fire and Ice attacks


        //remove one (behind the scenes it's calling Delegate.Remove)
        ManyAttacks -= FireAttack;

        ManyAttacks(401); //execute only Ice attack




        print("superAttack");
        SimpleAttack superAttack = FireAttack;

        superAttack += FireAttack;
        superAttack += IceAttack;
        superAttack(500);
        CopyAttack(600, superAttack);

    }	
}
