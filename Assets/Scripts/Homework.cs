using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Homework : MonoBehaviour
{
    float rotationSpeed, oldRotationSpeed;
    float minRotationSpeed = 180, maxRotationSpeed = 360; //Rotation speed changes to a random value between the min and the max values 
    int counter, health = 3; //The game starts with 3 healths. 3 misses and GAMEOVER
    bool canClick = true, isGameOver;
    [SerializeField] List<GameObject> healthBar;
    [SerializeField] GameObject hitText, missText, gameoverText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        counter = 0;
        rotationSpeed = minRotationSpeed;

        scoreText.text = "Score: " + counter;
        
        isGameOver = false;
        StartCoroutine(SetRotationSpeed());
        StartCoroutine(EndGame(15f));
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    //Changing rotation speed every second for a higher challenge
    IEnumerator SetRotationSpeed()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            oldRotationSpeed = rotationSpeed;

            //Change in the rotation speed cannot be less than 45 degrees per second
            while (Mathf.Abs(oldRotationSpeed - rotationSpeed) < 45)
            {
                rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            }
        }
    }

    //Displaying success and fail texts
    IEnumerator ShowText(GameObject text) 
    {
        text.SetActive(true);
        yield return new WaitForSeconds(.3f);
        text.SetActive(false);
    }

    //Adding a cooldown for the button
    IEnumerator ButtonCooldown(float cooldown)
    {
        canClick = false;
        yield return new WaitForSeconds(cooldown);
        canClick = true;
    }

    //The button
    public void HitButton()
    {
        if (canClick && !isGameOver)
        {
            StartCoroutine(ButtonCooldown(.3f));
            float angle = transform.eulerAngles.y % 90;
            if (angle < 20 || angle > 70)
            {
                counter++;
                scoreText.text = "Score: " + counter;
                StartCoroutine(ShowText(hitText));
            }
            else
            {
                health-= 1;
                if (health > 0)
                {
                    healthBar[health].SetActive(false);
                    StartCoroutine(ShowText(missText));
                }
                else
                {
                    healthBar[0].SetActive(false);
                    GameOver();
                }
            }
            Debug.Log(counter);
        }
    }

    void ScoreCounter()
    {

    }

    //GameOver
    public void GameOver()
    {
        isGameOver = true;
        hitText.SetActive(false);
        missText.SetActive(false);
        gameoverText.SetActive(true);
        gameObject.SetActive(false);
    }

    //Ends the game after a certain amount of time to prevent lame strategies
    IEnumerator EndGame(float gameDur)
    {
        yield return new WaitForSeconds(gameDur);
        GameOver();
    }
}