import z from "zod";

//#region ============= Get Account =============
export const getAccountResponseSchema = z.object({
  username: z.string(),
  email: z.string(),
  phoneNumber: z.preprocess(
    (value) => value ?? undefined,
    z.string().optional(),
  ),
  emailConfirmed: z.boolean(),
  phoneNumberConfirmed: z.boolean(),
});

export type GetAccountResponse = z.infer<typeof getAccountResponseSchema>;
//#endregion
