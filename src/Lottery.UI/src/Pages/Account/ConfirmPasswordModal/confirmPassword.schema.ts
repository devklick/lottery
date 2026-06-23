import z from "zod";

export const confirmPasswordRequestBodySchema = z.object({
  password: z.string(),
});

export type ConfirmPasswordRequestBody = z.infer<
  typeof confirmPasswordRequestBodySchema
>;

export const confirmPasswordResponseSchema = z.object({});
export type ConfirmPasswordResponse = z.infer<
  typeof confirmPasswordResponseSchema
>;
