const TodayEntries = document.querySelectorAll('.today-entry');
const OverTimeEntries = document.querySelectorAll('.overtime-entry');
const TodayEntryCounter = document.querySelector('.today-entries-counter');
const OverTimeEntryCounter = document.querySelector('.over-time-entry-counter');
let TodayEntryResult = 0;
let OverTimeEntryResult = 0; 

TodayEntries.forEach(e => {
    TodayEntryResult++;
})

OverTimeEntries.forEach(e => {
    OverTimeEntryResult++;
})

TodayEntryCounter.innerHTML = TodayEntryResult;
OverTimeEntryCounter.innerHTML = OverTimeEntryResult;