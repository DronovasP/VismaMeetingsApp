using VismaMeetings;
using System.Text.Json;

UserInterface UI = new UserInterface();
bool showMenu = true;
while (showMenu)
{
    showMenu = UI.MainMenu();
}
