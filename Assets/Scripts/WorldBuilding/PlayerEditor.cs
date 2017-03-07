using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEditor : MonoBehaviour {


    public BlockCreationController blockCreationController;

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Block tempBlock = new Block(blockCreationController.currentRotation, blockCreationController.currentColor);
                tempBlock.debugIsTrue = true;
                WorldEditor.SetBlock(hit, tempBlock, true);
                
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                WorldEditor.SetBlock(hit, new BlockEmpty(new Vector3(0, 0, 0), Color.white));

            }

        }
		if (Input.GetKeyDown(KeyCode.Mouse2))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100f))
			{
				WorldEditor.SetBlock(hit, new Block(new Vector3(0, 0, 0), Color.white),true);

			}

		}

        /*
        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 3,
            rot.y + Input.GetAxis("Mouse Y") * 3);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
        transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
        */
    }
}
