﻿namespace BackendAPI.Models;
public class Cart
{
    public int Id { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public ApplicationUser User { get; set; }


    public void AddItem(Location location, int countPeople, DateTime startTime, DateTime endTime)
    {

        TimeSpan totalHours = endTime - startTime;
        double totalHoursValue = totalHours.TotalHours;
        // หากต้องการให้ผลลัพธ์เป็นจำนวนชั่วโมงทั้งหมดที่เป็นจำนวนเต็ม
        int totalRoundedHours = (int)Math.Round(totalHoursValue);


        // ตรวจสอบโดยการ วนลูป ถ้าสินค้าที่ส่งมาไม่มีในตะกร้าให้เพิ่มเข้าไป
        if (Items.All(item => item.Locations.Id != location.Id))
        {
            //กำหนดค่าให้กับ ProductId โดยอัตโนมัติ
            Items.Add(new CartItem
            { 
                Locations = location,
                CountPeople = countPeople,
                StartTime = startTime,
                EndTime = endTime,
                TotalHour = totalRoundedHours,
            });
            }
        //รายการที่มีอยู่ ถ้ามีสินค้าในตะกร้าอยู่แล้วให้บวกจำนวนเพิ่มเข้าไป
        var existingItem = Items.FirstOrDefault(item => item.Locations.Id == location.Id);
        if (existingItem != null)
        {
            existingItem.CountPeople += countPeople;
            TimeSpan durationToAdd = endTime - existingItem.EndTime;
            existingItem.EndTime = existingItem.EndTime.Add(durationToAdd);

        }
    }

}


