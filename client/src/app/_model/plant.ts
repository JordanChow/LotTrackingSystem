import { Lot } from "./lot";

export interface Plant{
    location: number;
    lot?: Lot;
    state?: string;
    disable?: boolean;
}