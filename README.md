# Zoom Prompter

The following application was born to help Patients get prompted for a zoom meeting and allow doctors to consult via video conference.  

## Client Application

Receives messages via UDP and Rings on the clients device. Allowing them to accept or decline the Zoom Meeting

## Sender Application

You can send messages to configured devices.
Edit the Clients.json file to edit the device list
You could always use the Common.Zoom.Udp wrapper and create a secure webpage to send to the devices. The current WPF application if for clients that can run this type of infrastructure

### Prerequisites

- .net 4 and above on the client
- .net 4.8 on Sending App


## Getting Started

Compile the Code with Visual Studio or Build Agents
Create a MSI or copy the compile code 


## Network Requirements

- UDP traffic on port 1100
- Local Network 

## Future Enhancements

- Configuration of all settings

## Security

Please consider security when sending UDP on your network currently the only restriction on receiving traffic is the client will only pop if the URL meets certain criteria 
- Change URL to cater for your ORG.

Secondly only use this app if you are comfortable giving the end user access to the send messages. 

In production environment  send UDP messages from a secure webpage that is restricted to specific accounts

## Authors

- **Shawn Rosewarne** - _Initial work_ - [Drazzing](https://github.com/Drazzing)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details


## Acknowledgments

[Chris O'brien Lifehouse](https://www.mylifehouse.org.au/) for allowing this project to be shared for the good of patient care.
