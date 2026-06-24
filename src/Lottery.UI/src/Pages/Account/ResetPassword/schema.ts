import z from "zod";

export const resetPasswordPageQuerySchema = z.object({
  token: z.string(),
});

export type ResetPasswordPageQuery = z.infer<
  typeof resetPasswordPageQuerySchema
>;

export const resetPasswordRequestBodySchema = z.object({
  email: z.email(),
  password: z.string(),
  token: z.string(),
});

export type ResetPasswordRequestBody = z.infer<
  typeof resetPasswordRequestBodySchema
>;

export const resetPasswordFormSchema = resetPasswordRequestBodySchema.omit({
  token: true,
});
export type ResetPasswordForm = z.infer<typeof resetPasswordFormSchema>;

export const resetPasswordResponseSchema = z.object({});

export type ResetPasswordResponse = z.infer<typeof resetPasswordResponseSchema>;
