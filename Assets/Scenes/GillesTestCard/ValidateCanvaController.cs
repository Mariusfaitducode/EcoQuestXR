// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using System.IO;
// using TMPro;
// using UnityEngine.UI;
//
// public class ValidateCanvaController : MonoBehaviour
// {
//
//     void Start()
//     {
//         gameObject.SetActive(false);
//     }
//
//
//     void Update()
//     {
//
//         if (!transform.parent.GetComponent<DisplayCardController>().IsEnoughtCardSelected())
//         {
//
//             this.transform.Find("validate_button").GetComponentInChildren<Image>().color = new Color(0.433f, 0.433f, 0.433f, 1f);
//
//         }
//         else
//         {
//             this.transform.Find("validate_button").GetComponentInChildren<Image>().color = new Color(0.212f, 1f, 0.322f, 1f);
//         }
//
//     }
//
//
// }
