import z from "zod";

import { apiSuccessResponseSchema } from "../../common/schemas";

export const updateAccountRequestBodySchema = z
  .object({
    username: z.string().optional(),
    email: z.string().optional(),
    phoneNumber: z.string().optional(),
  })
  .superRefine(({ email, phoneNumber, username }, ctx) => {
    if (![email, phoneNumber, username].some(Boolean)) {
      ctx.addIssue({
        code: "custom",
        continue: true,
        path: ["email", "phone", "username"],
        message: "One or more property requires a value",
      });
    }
  });

export type UpdateAccountRequestBody = z.infer<
  typeof updateAccountRequestBodySchema
>;

export const updateAccountResponseSchema = z.object({
  username: apiSuccessResponseSchema.extend({ value: z.string() }),
  email: apiSuccessResponseSchema.extend({ value: z.string() }),
  phoneNumber: apiSuccessResponseSchema.extend({ value: z.string() }),
});

export type UpdateAccountResponse = z.infer<typeof updateAccountResponseSchema>;
