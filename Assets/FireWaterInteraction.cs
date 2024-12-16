using UnityEngine;

public class FireWaterInteraction : MonoBehaviour
{
    public ParticleSystem fireEffect;    // تأثير النار
    public ParticleSystem waterEffect;   //تأثير المياه
    public Light fireLight;              // إضاءة النار
    public Transform waterTransform;     //مكان المياه
    public float waterSpeed = 2f;        // سرعة تحرك المياه
    private bool isExtinguished = false; //  إطفاء النار

    void Update()
    {
        // تحريك المياه باتجاه النار
        if (!isExtinguished)
        {
            MoveWater();
        }

        // كيف المياه تسيطر على النار
        if (!isExtinguished && Vector3.Distance(waterTransform.position, fireEffect.transform.position) < 1.0f)
        {
            ExtinguishFire();
        }
    }

    void MoveWater()
    {
        // تحريك المياه لجهة النار
        Vector3 direction = (fireEffect.transform.position - waterTransform.position).normalized;
        waterTransform.position += direction * waterSpeed * Time.deltaTime;
    }

    void ExtinguishFire()
    {
        // إطفاء النار عند سيطرة المياه
        isExtinguished = true;

        // إيقاف شعلة النار
        if (fireEffect.isPlaying)
            fireEffect.Stop();

        // تقليل إضاءة النار شوية شوية
        StartCoroutine(FadeLight());
    }

    System.Collections.IEnumerator FadeLight()
    {
        float duration = 2.0f;
        float startIntensity = fireLight.intensity;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            fireLight.intensity = Mathf.Lerp(startIntensity, 0, t / duration);
            yield return null;
        }

        fireLight.intensity = 0;
    }
}
