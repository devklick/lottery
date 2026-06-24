import z from "zod";

export const forgotPasswordRequestBodySchema = z.object({
  email: z.email(),
});

export type ForgotPasswordRequestBody = z.infer<
  typeof forgotPasswordRequestBodySchema
>;

export const forgotPasswordResponseSchema = z.object({});

export type ForgotPasswordResponse = z.infer<
  typeof forgotPasswordResponseSchema
>;
