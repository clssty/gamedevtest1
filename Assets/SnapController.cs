using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapController : MonoBehaviour
{
    public List<Transform> snapPoints;         // Daftar snap point
    public List<Draggable> draggableobjects;   // Daftar objek yang dapat di-drag
    public float snapRange = 0.5f;             // Jarak maksimal untuk snap

    void Start() {
        foreach (Draggable draggable in draggableobjects) {
            draggable.dragEndedCallback = OnDragEnded;
        }
    }

    private void OnDragEnded(Draggable draggable) {
        float closestDistance = -1;
        Transform closestSnapPoint = null;

        // Mencari snap point terdekat
        foreach (Transform snapPoint in snapPoints) {
            float currentDistance = Vector2.Distance(draggable.transform.localPosition, snapPoint.localPosition);
            if (closestSnapPoint == null || currentDistance < closestDistance) {
                closestSnapPoint = snapPoint;
                closestDistance = currentDistance;
            }
        }

        // Jika snap point ditemukan dalam jangkauan yang ditentukan, pindahkan objek ke snap point dan hancurkan objek
        if (closestSnapPoint != null && closestDistance <= snapRange) {
            draggable.transform.localPosition = closestSnapPoint.localPosition;
            draggable.DestroyObject();  // Menghancurkan objek setelah mencapai snap point
        } else {
            draggable.targetPosition = draggable.originalPosition;
            draggable.shouldMoveBack = true;  // Mulai animasi kembali ke posisi semula
        }
    }
}
