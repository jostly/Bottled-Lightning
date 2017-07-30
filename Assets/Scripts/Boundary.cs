using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{

    private GameController gameController;

	void Start ()
    {
        gameController = GameController.Instance;

        if (gameController == null)
        {
            Debug.LogError("Cannot find GameController");
            Debug.Break();
        }
		
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameController.OutsideBounds(collision.gameObject);
    }

}
