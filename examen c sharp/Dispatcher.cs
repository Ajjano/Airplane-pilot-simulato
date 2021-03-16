using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Console;

namespace examen_c_sharp
{
    public delegate int AirlineSpeed();
    public delegate int AirlineHeight();
    public delegate void LendedHandler(string mes);
    public class Dispatcher
    {
        string nameDispatch;//имя диспетчера
        int correctionForWeatherCondition;//корректировка для погодных условий
        int valueCorrect;//величина корректировки
        int penaltyPoints;//шртрафные очки
        Random rnd = null;
        public Dispatcher(string name)
        {
            this.nameDispatch = name;
            rnd = new Random(DateTime.Now.Millisecond);
            correctionForWeatherCondition = rnd.Next(-200, 201);
        }
        public void ValueCorrection(AirlineSpeed airSpeed)
        {
            valueCorrect = 7 * airSpeed.Invoke() - correctionForWeatherCondition;
        }
        public void HeightAirline(AirlineHeight airHight, bool crashed)//начисление штрафных очков за высоту
        {
            try
            {
                if (airHight.Invoke() >= 50)
                {
                    if (Math.Abs((decimal)valueCorrect - (decimal)airHight.Invoke()) >= 300 && Math.Abs((decimal)valueCorrect - (decimal)airHight.Invoke()) < 600)
                    {
                        penaltyPoints += 25;
                    }
                    else if (Math.Abs((decimal)valueCorrect - (decimal)airHight.Invoke()) >= 600 && Math.Abs((decimal)valueCorrect - (decimal)airHight.Invoke()) < 1000)
                        penaltyPoints += 50;
                    else if (Math.Abs((decimal)valueCorrect - (decimal)airHight.Invoke()) >= 1000)
                    {
                        crashed = true;
                        throw new Exception("\nСамолет разбился");
                    }
                }
            }
            catch (Exception exc)
            {
                WriteLine(exc.Message);
                Environment.Exit(0);
            }

        }
        public int PenaltyPoints
        {
            get { return penaltyPoints; }
        }
        public int ValueCorrect
        {
            get { return valueCorrect; }
        }
        public override string ToString()
        {
            return $"Диспетчер {nameDispatch}, штрафные балы: {penaltyPoints}, корректировка: {valueCorrect}";
        }
    }


    public class Airline
    {
        bool startFl = false;
        AirlineSpeed airS;
        AirlineHeight airH;
        //public event LendedHandler Lend;
        bool crashed = false;
        LendedHandler del;
        List<Dispatcher> dispatchers;
        int speed, hight, penalty;
        Random rnd = new Random(DateTime.Now.Millisecond);
        public Airline()
        {
            WriteLine("Введите количество диспетчеров (не меньше 2): ");
            int amountDispatchers = int.Parse(ReadLine());
            while (amountDispatchers < 2)
            {
                WriteLine("2 или больше! Повторите ввод: ");
                amountDispatchers = int.Parse(ReadLine());
            }
            dispatchers = new List<Dispatcher>();
            for (int i = 0; i < amountDispatchers; i++)
            {
                WriteLine($"Введите имя диспетчера {i + 1}: ");
                string nameDisp = ReadLine();
                dispatchers.Add(new Dispatcher(nameDisp));
            }
            airS = ReturnSpeed;
            airH = ReturnHight;
        }
        public void RegisterHandler(LendedHandler del)
        {
            this.del = del;
        }
        public int ReturnSpeed()
        {
            return speed;
        }
        public int ReturnHight()
        {
            return hight;
        }
        public void StartFly()
        {
            if (del != null)
            {
                del("\nСамолет на старте");
            }
        }
        public void FinishFly()
        {
            if (del != null)
                del("\nКонец полета");
        }
        public void MaxHeight()
        {
            if (del != null)
                del("\nВы достигли максимальной высоты, идите на спад");
        }
        private void ShowMes(string mes)
        {
            WriteLine(mes);
        }
        public int ChangeSpeed(bool crash)
        {
            ConsoleKeyInfo key = ReadKey();
            try
            {
                if (key.Key == ConsoleKey.LeftArrow && key.Modifiers == 0)
                {
                    if (speed >= 50)
                        speed -= 50;
                    else
                    {
                        crash = true;
                        throw new Exception("\nСамолет разбился");

                    }
                }
                else if (key.Key == ConsoleKey.RightArrow && key.Modifiers == 0)
                {
                    if (speed < 1000)
                        speed += 50;
                    else if (speed == 1000)
                    {
                        WriteLine("\nВы достигли максимальной скорости, идите на спад");
                        ReadKey();
                    }
                    else
                    {
                        WriteLine("\nСлишком быстро, уменьште скорость");
                        ReadKey();
                    }
                }
                else if (key.Modifiers == ConsoleModifiers.Shift && key.Key == ConsoleKey.RightArrow)
                {
                    if (speed + 150 > 1000)
                    {
                        WriteLine("\nБлизко к максимальной скорости, уменьште прирост");
                        ReadKey();
                    }
                    speed += 150;
                }
                else if (key.Modifiers == ConsoleModifiers.Shift && key.Key == ConsoleKey.LeftArrow)
                {
                    if (speed - 150 <= 0)
                    {
                        crash = true;
                        throw new Exception("\nСамолет разбился");

                    }
                    else
                        speed -= 150;
                }

            }
            catch (Exception exc)
            {
                WriteLine(exc.Message);
                Environment.Exit(0);
            }
            return speed;
        }
        public int ChangeHidht(bool crash)
        {
            ConsoleKeyInfo key = ReadKey();
            try
            {
                if (key.Key == ConsoleKey.UpArrow && key.Modifiers == 0)
                {
                    hight += 250;
                }
                else if (key.Key == ConsoleKey.DownArrow && key.Modifiers == 0)
                {
                    if (hight - 250 < 0)
                    {
                        crash = true;
                        throw new Exception("Самолет разбился");

                    }
                    hight -= 250;
                }
                else if (key.Modifiers == ConsoleModifiers.Shift && key.Key == ConsoleKey.UpArrow)
                {
                    hight += 500;
                }
                else if (key.Modifiers == ConsoleModifiers.Shift && key.Key == ConsoleKey.DownArrow)
                {
                    if (hight - 500 <= 0)
                    {
                        crash = true;
                        throw new Exception("Самолет разбился");

                    }
                }
            }
            catch (Exception exc)
            {
                WriteLine(exc.Message);
                Environment.Exit(0);
            }
            return hight;
        }
        public int Penalty
        {
            get { return penalty; }
            set { penalty = value; }
        }
        public void CheckPenalty()
        {
            if (del != null) 
                del("Превышено количество штрафных, самолет разбился");
        }
        public void Flight()
        {
            
            RegisterHandler(new LendedHandler(ShowMes));
            StartFly();
            Thread.Sleep(1000);
            int currPenalt = 0;
            while (!crashed)
            {
                Clear();
                foreach (var item in dispatchers)
                {

                    WriteLine(item);
                }

                WriteLine($"Общие штрафные очки: {penalty}");
                WriteLine($"Текущая высота: {hight}");
                WriteLine($"Текущая скорость: {speed}");
                if (speed >= 1000)
                    MaxHeight();
                
                ChangeHidht(crashed);
                ChangeSpeed(crashed);
                
                if (speed > 0) startFl = true;
                currPenalt = 0;
                foreach (var item in dispatchers)
                {

                    item.ValueCorrection(airS);
                    item.HeightAirline(airH, crashed);
                    currPenalt += item.PenaltyPoints;
                }
               
                Penalty = currPenalt;
                if (Penalty >= 1000)
                {
                    CheckPenalty();
                    break;
                }
                try
                {
                    if (speed == 0 && hight == 0 && startFl == true)
                    {
                        WriteLine("\nВы посадили самолет");
                        break;
                    }
                    else if (speed == 0 && hight != 0 && startFl == true)
                    {
                        throw new Exception("\nСамолет разбился");
                    }
                }catch(Exception exc)
                { WriteLine(exc.Message); }
            }
            FinishFly();
        }
    }
}
