using System;
using System.Collections.Generic;

interface IWeapon
{
    string Name { get; }
    int Range { get; }
    int Power { get; }
    int Weight { get; }
}

class Rifle : IWeapon
{
    public string Name { get; } = "Rifle";
    public int Range { get; } = 500;
    public int Power { get; } = 30;
    public int Weight { get; } = 5;
}

class Pistol : IWeapon
{
    public string Name { get; } = "Pistol";
    public int Range { get; } = 100;
    public int Power { get; } = 15;
    public int Weight { get; } = 2;
}

class MachineGun : IWeapon
{
    public string Name { get; } = "Machine Gun";
    public int Range { get; } = 800;
    public int Power { get; } = 40;
    public int Weight { get; } = 10;
}

interface IWeaponAccessory
{
    string Name { get; }
    int Price { get; }
    int Weight { get; }
    int Power { get; }
}

class WeaponWithAccessory : IWeapon
{
    private readonly IWeapon baseWeapon;
    private readonly IWeaponAccessory accessory;

    public WeaponWithAccessory(IWeapon baseWeapon, IWeaponAccessory accessory)
    {
        this.baseWeapon = baseWeapon;
        this.accessory = accessory;
    }

    public string Name => $"{baseWeapon.Name} with {accessory.Name}";
    public int Range => baseWeapon.Range;
    public int Power => baseWeapon.Power + accessory.Power;
    public int Weight => baseWeapon.Weight + accessory.Weight;
}

class ArmoryFacade
{
    private readonly List<IWeapon> weapons = new List<IWeapon>();

    public void AddWeapon(IWeapon weapon)
    {
        weapons.Add(weapon);
    }

    public void AddAccessoryToWeapon(IWeapon weapon, IWeaponAccessory accessory)
    {
        var weaponWithAccessory = new WeaponWithAccessory(weapon, accessory);
        weapons.Add(weaponWithAccessory);
    }

    public int CalculateTotalCost()
    {
        int totalCost = 0;
        foreach (var weapon in weapons)
        {
            totalCost += CalculateWeaponCost(weapon);
        }
        return totalCost;
    }

    private int CalculateWeaponCost(IWeapon weapon)
    {
        return weapon.Power * weapon.Range;
    }

    public IWeapon GetMostExpensiveWeapon()
    {
        if (weapons.Count == 0)
            return null;

        IWeapon mostExpensiveWeapon = weapons[0];
        foreach (var weapon in weapons)
        {
            if (CalculateWeaponCost(weapon) > CalculateWeaponCost(mostExpensiveWeapon))
            {
                mostExpensiveWeapon = weapon;
            }
        }
        return mostExpensiveWeapon;
    }

    public void DepreciateWeapons(int depreciationYear)
    {
        foreach (var weapon in weapons)
        {
            // Моделюємо знос на підставі року випуску
            int yearsInUse = DateTime.Now.Year - depreciationYear;
            // Зменшуємо вартість на 10% за кожен рік використання
            int depreciationRate = 10;
            int depreciation = (int)(CalculateWeaponCost(weapon) * (yearsInUse * depreciationRate / 100.0));
            Console.WriteLine($"Depreciating {weapon.Name} by {depreciation}$ due to {yearsInUse} years of use.");
        }
    }
}

class Program
{
    static void Main()
    {
        ArmoryFacade armory = new ArmoryFacade();

        armory.AddWeapon(new Rifle());
        armory.AddWeapon(new Pistol());
        armory.AddWeapon(new MachineGun());

        armory.AddAccessoryToWeapon(armory.GetMostExpensiveWeapon(), new Optics());
        armory.AddAccessoryToWeapon(armory.GetMostExpensiveWeapon(), new Silencer());

        Console.WriteLine($"Total cost of the armory: {armory.CalculateTotalCost()}$");
        Console.WriteLine($"Most expensive weapon: {armory.GetMostExpensiveWeapon().Name}");

        armory.DepreciateWeapons(2010);
    }
}

class Optics : IWeaponAccessory
{
    public string Name => "Optics";
    public int Price => 100;
    public int Power => 5;
    public int Weight => 2;
}

class Silencer : IWeaponAccessory
{
    public string Name => "Silencer";
    public int Price => 50;
    public int Power => 2;
    public int Weight => 1;
}
