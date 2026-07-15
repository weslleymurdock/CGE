string MAIN_MENU = @"""
#################################################
###### Card Game Engine Sample Application ######
#################################################
######                                     ######
######          (s/S). Start Game          ######
######          (e/E). Exit Game           ######
######                                     ######
#################################################
"""; 
ConsoleKeyInfo input;
do
{
    Console.WriteLine(MAIN_MENU);
    input = Console.ReadKey();
}while (input.Key != ConsoleKey.Escape && input.Key != ConsoleKey.E);
