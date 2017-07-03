# SmtpClient.WebJobs.Extensions
Utiltity Azure Web Jobs Extension Binding for using SmtpClient

#Smtp
A binding that will send a email message using an SmtpClient

Example of sending a message from an order object sent via a queue
```
public static void ProcessOrder_Declarative(
    [QueueTrigger(@"sample-orders")] Order order,
    [Smtp(
        To ="jason@jasonhaley.com", 
        From ="jason@jasonhaley.com", 
        Subject ="testing", 
        Body ="this is a test")]
        string message)
{
    // You can set addtional message properties here
}
```
