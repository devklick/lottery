import z from "zod";

export const deleteAccountResponseSchema = z.object({});
export type DeleteAccountResponse = z.infer<typeof deleteAccountResponseSchema>;
