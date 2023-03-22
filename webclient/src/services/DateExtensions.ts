export function ConvertDateToInputString(value: Date | null) {
    if (value === null || !isFinite(value.getTime())) {
        return "";
    }

    return value.toISOString().slice(0, 10);
}