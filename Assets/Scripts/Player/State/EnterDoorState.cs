using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class EnterDoorState : State
{
    public override bool CanEnter()
    {
        if (player.exitdoor == null)
        {
            return false;
        }
        return true;
    }
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(EnterDoor());
    }
    private IEnumerator EnterDoor()
    {
        transform.position = new Vector2(player.exitdoor.transform.position.x + Tile.Width / 2f, player.exitdoor.transform.position.y);
        player.animator.Play("EnterDoor");
        player.animator.fps = 12;
        Color color = player.renderer.color;
        float animatonLength = player.animator.GetAnimationLength("EnterDoor");
        float t = 0;
        while (t <= animatonLength)
        {
            t += Time.deltaTime;
            player.renderer.color = Color.Lerp(color, Color.black, (float)t / animatonLength);
            yield return null;
        }
        Scene scene = SceneManager.GetActiveScene();
        string tmp = scene.name;
        if (tmp.Equals("Game"))
        {
            tmp = "Center";
            int x = PlayerPrefs.GetInt("level",0);
            x = x + 1;
            PlayerPrefs.SetInt("level", x);
            int score = player.inventory.goldAmount;
            PlayerPrefs.GetInt("level " + x, 0);
            PlayerPrefs.SetInt("level " + x, score);
            int sum = PlayerPrefs.GetInt("sum",0);
            sum = sum + score;
            PlayerPrefs.SetInt("sum", sum);
            int max = PlayerPrefs.GetInt("max", 0);
            PlayerPrefs.SetInt("max", Math.Max(sum,max));
        }
        else
        {
            tmp = "Game";
        }
        SceneManager.LoadScene(tmp);
    }
}
