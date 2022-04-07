using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//New as of Feb.25rd

public class Service : MonoBehaviour
{
    public GameObject customerInService;
    public Transform customerExitPlace;

    public float serviceRateAsCustomersPerHour = 20; // customer/hour
    public float interServiceTimeInHours; // = 1.0 / Service time of customer Per Hour;
    private float interServiceTimeInMinutes;
    private float interServiceTimeInSeconds;

    public bool generateServices = false;
    // minimum and maximum interservice time in seconds
    public float minInterServiceTimeInSeconds = 3;
    public float maxInterServiceTimeInSeconds = 60;
    public System.Array sizeValues, baseValues, additionsValues;
    public System.Type type;
    public Size orderedSize;
    public Base orderedBase;
    public List<Additions> orderedAdditions;
    public Order order;
    Queue queueManager;

    public Text Timer;
    public Text orderText;
    public float elapsedSeconds = 0f;

    public float timeScale = 1;

    public Slider sliderTScale;
    public Dropdown sizeDropDown;
    public Dropdown baseDropDown;
    public Button caramelBtn, chocolateBtn, strawberryBtn, vanillaBtn, mapleBtn, peppermintBtn;//, submitBtn;

    private string createOrder = "";
    private string orderList = "";
    private string orderMessage = "Order is Correct!";

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    // Start is called before the first frame update
    void Start()
    {
        interServiceTimeInHours = 1.0f / serviceRateAsCustomersPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;
        CreateOrder();
    }

    private void Update()
    {
        timeScale = sliderTScale.value;
        //submitBtn.onClick.AddListener(CompareOrder);
    }

    private void FixedUpdate()
    {
        elapsedSeconds += Time.deltaTime;
        Timer.text = "Total time in seconds: " + elapsedSeconds.ToString();
        
    }
    private void OnTriggerEnter(Collider other)
    {
#if DEBUG_SP
        print("ServiceProcess.OnTriggerEnter:otherID=" + other.gameObject.GetInstanceID());
#endif

        if (other.gameObject.tag == "Customer")
        {
            customerInService = other.gameObject;
            customerInService.GetComponent<CustomerController>().SetInService(true);

            GetRandomOrderValues();
            order = new Order(orderedAdditions, orderedSize, orderedBase);

            //generateServices = true;
            //StartCoroutine(GenerateServices());
        }
    }

    public void GetRandomOrderValues()
    {
        orderedAdditions.Clear();
        orderText.text = "Order: ";
        int index;
        System.Random random = new System.Random();
        type = typeof(Size);
        sizeValues = type.GetEnumValues();
        index = random.Next(sizeValues.Length);
        orderedSize = (Size)sizeValues.GetValue(index);
        orderText.text += ("\nSize: " + orderedSize);

        type = typeof(Base);
        baseValues = type.GetEnumValues();
        index = random.Next(baseValues.Length);
        orderedBase = (Base)baseValues.GetValue(index);
        orderText.text += ("\nBase: " + orderedBase);

        type = typeof(Additions);
        additionsValues = type.GetEnumValues();
        int numberOfAdditions = random.Next(1, 5);
        Additions addition;
        for (int i = 0; i <= numberOfAdditions; i++)
        {
            // Get random num from the Additions enum
            index = random.Next(additionsValues.Length);
            addition = (Additions)additionsValues.GetValue(index);
            orderedAdditions.Add(addition);
        }
        orderText.text += ("\nAdditions: ");
        foreach (var add in orderedAdditions)
        {
            orderText.text += "\n" + add;
        }
        orderList = orderText.text;
    }

    IEnumerator GenerateServices()
    {
        GetRandomOrderValues();
        order = new Order(orderedAdditions, orderedSize, orderedBase);

        
        while (generateServices)
        {
            float timeToNextServiceInSec = interServiceTimeInSeconds;
            switch (serviceIntervalTimeStrategy)
            {
                case ServiceIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                case ServiceIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextServiceInSec = Random.Range(minInterServiceTimeInSeconds, maxInterServiceTimeInSeconds);
                    break;
                case ServiceIntervalTimeStrategy.ExponentialIntervalTime:
                    float U = Random.value;
                    float Lambda = 1 / serviceRateAsCustomersPerHour;
                    timeToNextServiceInSec = GetExp(U, Lambda);
                    break;
                case ServiceIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                default:
                    print("No acceptable ServiceIntervalTimeStrategy:" + serviceIntervalTimeStrategy);
                    break;

            }

            generateServices = false;
            yield return new WaitForSeconds(timeToNextServiceInSec / timeScale);
        }
        customerInService.GetComponent<CustomerController>().ExitService(customerExitPlace);
        customerInService = null;

    }
    private void OnDrawGizmos()
    {
        if (customerInService)
        {
            Renderer r = customerInService.GetComponent<Renderer>();
            r.material.color = Color.green;
        }
    }

    static float GetExp(float u, float lambda)
    {
        //throw new NotImplementedException();
        return -Mathf.Log(1 - u) / lambda;
    }

    public void CreateOrder()
    {
        Debug.Log("creating order");
        sizeDropDown = GetComponent<Dropdown>();
        sizeDropDown.onValueChanged.AddListener(delegate
        {
            CreateOrderList($"n/Size :n/{sizeDropDown}");
        });

        baseDropDown = GetComponent<Dropdown>();
        baseDropDown.onValueChanged.AddListener(delegate
        {
            CreateOrderList($"n/Base :n/{sizeDropDown}");
        });

        caramelBtn.onClick.AddListener(CaramelOnClick);
        chocolateBtn.onClick.AddListener(ChocolatelOnClick);
        strawberryBtn.onClick.AddListener(StrawberryOnClick);
        vanillaBtn.onClick.AddListener(VanillaOnClick);
        mapleBtn.onClick.AddListener(MapleOnClick);
        peppermintBtn.onClick.AddListener(PeppermintOnClick);

    }
    void CaramelOnClick()
    {
        Debug.Log("Caramel");
        createOrder += "n/Caramel";
    }
    void ChocolatelOnClick()
    {
        Debug.Log("Chocolate");
        createOrder += "n/Chocolate";
    }

    void StrawberryOnClick()
    {
        Debug.Log("Strawberry");
        createOrder += "n/Strawberry";
    }

    void VanillaOnClick()
    {
        Debug.Log("Vanilla");
        createOrder += "n/Vanilla";
    }

    void MapleOnClick()
    {
        Debug.Log("Maple");
        createOrder += "n/Maple";
    }

    void PeppermintOnClick()
    {
        Debug.Log("Peppermint");
        createOrder += "n/Peppermint";
    }

    void CreateOrderList(string value)
    {
        createOrder += value;
    }

    public void CompareOrder()
    {
        string[] splitOrder = createOrder.Split(char.Parse("n/"));
        string[] splitList = orderList.Split(char.Parse("n/"));
        bool[] correct = new bool[splitList.Length];

        foreach (var i in orderList)
        {
            if (splitOrder[i] == splitList[i])
            {
                correct[i] = true;
            }
            else
            {
                correct[i] = false;
            }
        }

        Debug.Log(correct);

        

        for (int i = 0; i < correct.Length; i++)
        {
            if (!correct[i])
            {
                orderMessage = "Order is wrong!";
                break;
            }
        }



    }

}

