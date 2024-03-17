import { z } from "zod";

export const States = {
  Enabled: "Enabled",
  Disabled: "Disabled",
} as const;

export const AllStates = Object.keys(States);

export const stateSchema = z.nativeEnum(States);

export type State = z.infer<typeof stateSchema>;
