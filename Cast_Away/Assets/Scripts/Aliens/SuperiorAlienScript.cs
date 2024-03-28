using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperiorAlienScript : BaseAlienScript
{

    public SuperiorAlienScript() {
        this.Health = 200;
        this.baseHealth = 200;
        this.Species = "Superior Alien";
        this.Aggression = 150;
        this.baseAggression = 150;
        this.attackDamage = 70;
        this.sprite = Resources.Load<Sprite>("AlienSprites/Comic Battle Royale/2D Character - Alien/Variant C/Sprites/Character/walk/side/01");
        this.attacks = new List<EnemyAttack>();
        this.acts = new List<Move>();
        attacks.Add(new EnemyAttack("Hammer Spin", 100, "Physical", 20f, 3, (1f,1f), "Hammer", 1));
        attacks.Add(new EnemyAttack("Acid Rain", 125, "Chemical", 30f, 10, (0.5f,1f), "Green ball", 2));
        acts.Add(new Move("Plead", 10, "Emotional", "", "", ""));
        acts.Add(new Move("Appeal", 10, "Emotional", "", "", ""));
        acts.Add(new Move("Flex Muscles", 10, "Emotional", "", "", ""));
        acts.Add(new Move("Challenge Authority", 20, "Emotional", "", "", ""));
    }
    public override double Attack()
    {
        base.Attack();

        return this.attackDamage;
    }
}