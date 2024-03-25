using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {
    Start, PlayerAction, PlayerAttack, PlayerAct, AlienMove, Busy
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] KeyCode ConfirmKey;
    [SerializeField] KeyCode RejectKey;

    [SerializeField] PlayerBattleHud playerHud;
    [SerializeField] PlayerBattleUnit playerUnit;
    [SerializeField] AlienBattleHud alienHud;
    [SerializeField] AlienBattleUnit alienUnit;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    int currentAttack;
    int currentAct;

    private void Start() {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle() {
        //Setup Player
        playerUnit.Setup();
        playerHud.SetData(playerUnit.player);
        //Setup Alien
        alienUnit.Setup();
        alienHud.SetData(alienUnit.alien);
        dialogBox.EnableAttackSelector(false);
        dialogBox.EnableActSelector(false);

        //Setup Moves
        dialogBox.SetAttackNames(playerUnit.player.attacks);

        //Setup Acts
        dialogBox.SetActNames(alienUnit.alien.acts);

        //Run dialog Text
        yield return dialogBox.TypeDialog($"You were stopped by {alienUnit.alien.Species}!");
        //Wait for a second
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction() {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAttackSelector(false);
        dialogBox.EnableActSelector(false);
    }

    void PlayerAttack() {
        state = BattleState.PlayerAttack;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableAttackSelector(true);
        currentAttack = 0;
    }

    IEnumerator PerformPlayerAttack() {
        state = BattleState.Busy;

        var attack = playerUnit.player.attacks[currentAttack];
        yield return dialogBox.TypeDialog($"{playerUnit.player.Name} used {attack.MoveName}");
        yield return new WaitForSeconds(1f);

        bool isDead = alienUnit.alien.TakeDamage(attack);
        yield return alienHud.UpdateHP();
        if (isDead) {
            yield return dialogBox.TypeDialog($"{alienUnit.alien.Species} is no longer moving...");
        } else {
            StartCoroutine(AlienAttack());
        }
    }

    IEnumerator PerformPlayerAct() {
        state = BattleState.Busy;

        var act = alienUnit.alien.acts[currentAct];
        yield return dialogBox.TypeDialog($"{playerUnit.player.Name} used {act.MoveName}");
        yield return new WaitForSeconds(1f);

        bool isPacified = alienUnit.alien.TakePacify(act);
        yield return alienHud.UpdateA();
        if (isPacified) {
            yield return dialogBox.TypeDialog($"{alienUnit.alien.Species} no longer wants to fight");
        } else {
            StartCoroutine(AlienAttack());
        }
    }

    IEnumerator AlienAttack() {
        state = BattleState.AlienMove;

        var attack = alienUnit.alien.generateMove();
        yield return dialogBox.TypeDialog($"{alienUnit.alien.Species} used {attack.MoveName}");
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.player.TakeDamage(attack);
        yield return playerHud.UpdateHP();
        if (isDead) {
            yield return dialogBox.TypeDialog($"{playerUnit.player.Name} Died");
        } else {
            PlayerAction();
        }
    }

    void PlayerAct() {
        state = BattleState.PlayerAct;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActSelector(true);
        currentAct = 0;
    }

    private void Update() {
        if (state == BattleState.PlayerAction) {
            HandleActionSelection();
        } else if (state == BattleState.PlayerAttack) {
            HandleAttackSelection();
        } else if (state == BattleState.PlayerAct) {
            HandleActSelection();
        }
    }

       void HandleActionSelection() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentAction < 1)
                ++currentAction;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(ConfirmKey)) {
            if (currentAction == 0) {
                //Fight
                PlayerAttack();
            } else if (currentAction == 1) {
                //Act
                PlayerAct();
            }
        }
    }


    void HandleActSelection() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentAct < alienUnit.alien.acts.Count - 1)
                ++currentAct;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currentAct > 0)
                --currentAct;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentAct < alienUnit.alien.acts.Count -2)
                currentAct += 2;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentAct > 1)
                currentAct -= 2;
        }

        if (Input.GetKeyDown(RejectKey)) {
            PlayerAction();
        }

        dialogBox.UpdateActSelection(currentAct, alienUnit.alien.acts[currentAct]);

        if (Input.GetKeyDown(ConfirmKey)) {
            dialogBox.EnableActSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAct());
        }
    }

 
    void HandleAttackSelection() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentAttack < playerUnit.player.attacks.Count - 1)
                ++currentAttack;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currentAttack > 0)
                --currentAttack;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentAttack < playerUnit.player.attacks.Count -2)
                currentAttack += 2;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentAttack > 1)
                currentAttack -= 2;
        }

        if (Input.GetKeyDown(RejectKey)) {
            PlayerAction();
        }

        dialogBox.UpdateAttackSelection(currentAttack, playerUnit.player.attacks[currentAttack]);
    
        if (Input.GetKeyDown(ConfirmKey)) {
            dialogBox.EnableAttackSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAttack());
        }
    }

}

