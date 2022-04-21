﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//New as of Feb.25rd

public class Service : MonoBehaviour
{
    public int numOrderCompleted;

    public GameObject customerInService;
    public GameObject GameController;
    public Text currentOrderText;
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

    //public Text Timer;
    public Text orderText;
    public float elapsedSeconds = 0f;

    public float timeScale = 1;

    public Slider sliderTScale;
    public Dropdown sizeDropDown;
    public Dropdown baseDropDown;
    public Button caramelBtn, chocolateBtn, strawberryBtn, vanillaBtn, mapleBtn, peppermintBtn, submitBtn;

    private string UserCreatedOrder = "";
    private string RandOrderItems = "";
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

        submitBtn.onClick.AddListener(CompareOrder);

        sizeDropDown.value = 0;
        baseDropDown.value = 0;

        caramelBtn.onClick.AddListener(CaramelOnClick);
        chocolateBtn.onClick.AddListener(ChocolatelOnClick);
        strawberryBtn.onClick.AddListener(StrawberryOnClick);
        vanillaBtn.onClick.AddListener(VanillaOnClick);
        mapleBtn.onClick.AddListener(MapleOnClick);
        peppermintBtn.onClick.AddListener(PeppermintOnClick);
        GameController = GameObject.FindGameObjectWithTag("GameController");
    }

    private void Update()
    {
        //timeScale = sliderTScale.value;

    }

    private void FixedUpdate()
    {
        elapsedSeconds += Time.deltaTime;
        //Timer.text = "Total time in seconds: " + elapsedSeconds.ToString();

        currentOrderText.text = UserCreatedOrder;

        if (elapsedSeconds > 100)
        {
            if(numOrderCompleted == 5)
            {
                GameController.GetComponent<SceneController>().LoadVictory();
            }
            else
            {
                GameController.GetComponent<SceneController>().LoadLose();
            }
        }
        else if(numOrderCompleted == 5)
        {
            GameController.GetComponent<SceneController>().LoadVictory();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            if (customerInService != other.gameObject)
            {
                customerInService = other.gameObject;
                customerInService.GetComponent<CustomerController>().SetInService(true);
                Debug.Log("Customer is Here: " + other.gameObject.name);
                GetRandomOrderValues();
                order = new Order(orderedAdditions, orderedSize, orderedBase);

                //generateServices = true;
                //StartCoroutine(GenerateServices());
            }
        }
#if DEBUG_SP
        print("ServiceProcess.OnTriggerEnter:otherID=" + other.gameObject.GetInstanceID());
#endif


    }

    public void GetRandomOrderValues()
    {
        RandOrderItems = "";
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

            RandOrderItems += addition + "\n";

        }

        RandOrderItems += orderedSize + "\n";
        RandOrderItems += orderedBase;
        Debug.Log(RandOrderItems);
        orderText.text += ("\nAdditions: ");
        foreach (var add in orderedAdditions)
        {
            orderText.text += "\n" + add;
        }

        //orderList = orderText.text;
    }

    IEnumerator GenerateServices()
    {
        //GetRandomOrderValues();
        //order = new Order(orderedAdditions, orderedSize, orderedBase);

        
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

  
    void CaramelOnClick()
    {
        UserCreatedOrder += "Caramel\n";
    }
    void ChocolatelOnClick()
    {
        UserCreatedOrder += "Chocolate\n";
    }

    void StrawberryOnClick()
    {
        UserCreatedOrder += "Strawberry\n";
    }

    void VanillaOnClick()
    {
        UserCreatedOrder += "Vanilla\n";
    }

    void MapleOnClick()
    {
        UserCreatedOrder += "Maple\n";
    }

    void PeppermintOnClick()
    {
        UserCreatedOrder += "Peppermint\n";
    }


    public void CompareOrder()
    {
        UserCreatedOrder += sizeDropDown.options[sizeDropDown.value].text + "\n";
        UserCreatedOrder += baseDropDown.options[baseDropDown.value].text;
        
        sizeDropDown.value = 0;
        baseDropDown.value = 0;

        string[] splitOrder = UserCreatedOrder.Split(char.Parse("\n"));
        string[] RandSplitList = RandOrderItems.Split(char.Parse("\n"));

        Debug.Log(splitOrder.Length);
        Debug.Log(RandSplitList.Length);

        bool[] correct = new bool[RandSplitList.Length];

        if (splitOrder.Length == RandSplitList.Length)
        {
            Debug.Log(RandOrderItems);
            Debug.Log(UserCreatedOrder);
            for (int i = 0; i < RandSplitList.Length; i++)
            {
                if (RandOrderItems.Contains(splitOrder[i]))
                {
                    correct[i] = true;
                }
                else
                {
                    correct[i] = false;
                }
            }
        }

        int x = 0;
        for (int i = 0; i < correct.Length; i++)
        {
            //Debug.Log(correct[i]);
            if (!correct[i])
            {
                x += 1;
                orderText.text = "Order is wrong!";
                customerInService.GetComponent<CustomerController>().ChangeState(CustomerController.CustomerState.Serviced);
                UserCreatedOrder = "";
                break;
            }
            
        }
        if (x == 0)
        {
            orderText.text = "Order Completed!";
            customerInService.GetComponent<CustomerController>().ChangeState(CustomerController.CustomerState.Serviced);
            numOrderCompleted += 1;
            UserCreatedOrder = "";
        }



    }

}

