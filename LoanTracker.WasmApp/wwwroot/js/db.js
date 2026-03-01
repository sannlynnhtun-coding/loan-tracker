// Use the Dexie instance from the script tag in index.html
const Dexie = window.Dexie;

export const db = new Dexie('LoanTrackerDB');

db.version(1).stores({
    customers: '++customerId, customerName, nrc',
    loanTypes: '++loanTypeId, loanTypeName',
    loans: '++loanId, customerId, loanTypeId, status',
    paymentSchedules: '++scheduleId, loanId, status',
    payments: '++paymentId, scheduleId, status',
    lateFees: '++lateFeeId, scheduleId'
});

export async function clearAll() {
    await db.customers.clear();
    await db.loanTypes.clear();
    await db.loans.clear();
    await db.paymentSchedules.clear();
    await db.payments.clear();
    await db.lateFees.clear();
}

export async function getSchedulesByLoanId(loanId) {
    return await db.paymentSchedules.where('loanId').equals(loanId).toArray();
}

// Ensure functions are globally accessible for Blazor InvokeAsync
window.dbInstance = db;
window.clearAll = clearAll;
window.getSchedulesByLoanId = getSchedulesByLoanId;
