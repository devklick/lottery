import z from "zod";

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
  username: z.object({
    value: z.string(),
    errors: z.array(z.string()).optional(),
    status: z.number(),
  }),
  email: z.object({
    value: z.string(),
    errors: z.array(z.string()).optional(),
    status: z.number(),
  }),
  phoneNumber: z.object({
    value: z.string(),
    errors: z.array(z.string()).optional(),
    status: z.number(),
  }),
});

export type UpdateAccountResponse = z.infer<typeof updateAccountResponseSchema>;
