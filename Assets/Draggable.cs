using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public delegate void DragEndedDelegate(Draggable draggableObject);
    public DragEndedDelegate dragEndedCallback;

    private bool isDragged = false;
    private Vector3 mouseDragStartPosistion;
    private Vector3 spriteDragStartPosition;

    // Menyimpan posisi awal untuk kembali jika tidak berada di snap point
    public Vector3 originalPosition { get; private set; }

    // Kecepatan pergerakan kembali ke posisi semula
    private float initialMoveBackSpeed = 5f;
    private float moveBackSpeed;
    public bool shouldMoveBack = false;
    public Vector3 targetPosition;

    private void OnMouseDown() {
        isDragged = true;
        mouseDragStartPosistion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteDragStartPosition = transform.localPosition;
        originalPosition = transform.localPosition;  // Menyimpan posisi awal
    }

    private void OnMouseDrag() {
        if (isDragged) {
            transform.localPosition = spriteDragStartPosition + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosistion);
        }
    }

    private void OnMouseUp() {
        isDragged = false;
        dragEndedCallback?.Invoke(this);

        // Jika objek tidak mendekati snap point, mulai animasi untuk kembali
        if (transform.localPosition != targetPosition) {
            shouldMoveBack = true;
            targetPosition = originalPosition;
        }
    }

    private void Update() {
        // Menggerakkan objek secara perlahan kembali ke posisi semula
        if (shouldMoveBack) {
            float distanceToTarget = Vector3.Distance(transform.localPosition, targetPosition);

            // Menyesuaikan kecepatan berdasarkan jarak (semakin dekat, semakin lambat)
            moveBackSpeed = Mathf.Lerp(0, initialMoveBackSpeed, distanceToTarget);

            // Menggerakkan objek menuju target menggunakan moveBackSpeed yang sudah disesuaikan
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveBackSpeed * Time.deltaTime);

            if (transform.localPosition == targetPosition) {
                shouldMoveBack = false;  // Berhenti jika sudah di posisi semula
            }
        }
    }

    public void DestroyObject() {
        Debug.Log($"Object {gameObject.name} has been destroyed.");
        Destroy(gameObject);  // Menghancurkan objek ini
    }

    
}
