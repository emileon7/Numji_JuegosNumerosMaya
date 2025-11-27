using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int valor;
    public bool esSpawner = true;
    private GameObject itemCopia;
    private Transform OriginalContainer;
    private CanvasGroup canvasGroup;
    private GameObject Container;

    void Start(){

        OriginalContainer = transform.parent;
        
        

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if ( esSpawner) {

            itemCopia = Instantiate(gameObject, transform.position, transform.rotation);
            itemCopia.GetComponent<Draggable>().esSpawner = false; //no es esSp
        }
        else
        {
            itemCopia = this.gameObject;
        }

         
        
        itemCopia.name = gameObject.name; //Quita el (Clone) del nombre
        itemCopia.transform.SetParent(transform.root); //Lo pone en la raíz de la jerarquía para que no se oculte
        //itemCopia.transform.SetParent(OriginalContainer.parent); //Lo pone al mismo nivel que el contenedor original
        canvasGroup = itemCopia.GetComponent<CanvasGroup>(); 

        if (canvasGroup == null)
        {
            canvasGroup = itemCopia.AddComponent<CanvasGroup>();
        }
        canvasGroup.blocksRaycasts = false; //Desactiva los raycasts para que no interfiera con otros elementos
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemCopia != null)
        {
            // ¡Mover el CLON (itemCopia), no el original (transform)!
            itemCopia.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemCopia == null) return;
        
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true; //Restablece los raycasts
        }

        if(eventData.pointerEnter != null)
        {
            if (eventData.pointerEnter.CompareTag("Tablilla"))
            {
                itemCopia.transform.SetParent(eventData.pointerEnter.transform); //Hace hijo del objeto sobre el que se soltó
                Debug.Log("Soltado sobre una Tablilla: " + eventData.pointerEnter.name);
            }
            else if (eventData.pointerEnter.CompareTag("CestoDeBasura"))
            {
                Destroy(itemCopia);
                Debug.Log("Eliminado en el CestoDeBasura: " + gameObject.name);
            }
            else
            {
                Destroy(itemCopia);
            }
        }
        else
        {
            Destroy(itemCopia);
        }
        

        //if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Tablilla"))
        //{
            
        //    itemCopia.transform.SetParent(eventData.pointerEnter.transform); //Hace hijo del objeto sobre el que se soltó
        //    Debug.Log("Soltado sobre una Tablilla: " + eventData.pointerEnter.name);
        //}
        //else if(eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("CestoDeBasura"))
        //{
        //    Destroy(itemCopia);
        //    Debug.Log("Eliminado en el CestoDeBasura: " + gameObject.name);
        //}
        //else
        //{
          
        //    itemCopia.transform.SetParent(OriginalContainer); //Vuelve a su contenedor original
        //    Debug.Log("No es la tablilla: " + gameObject.name);

        //}

        Debug.Log("Terminó de arrastrar: " + gameObject.name);
        itemCopia = null; // Limpia la referencia al clon
        canvasGroup = null; // Limpia la referencia al CanvasGroup


    }
}