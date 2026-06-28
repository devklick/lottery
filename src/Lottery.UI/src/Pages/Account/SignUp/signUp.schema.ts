import { z } from "zod";
export const signUpRequestSchema = z
  .object({
    email: z
      .email()
      .describe(
        "A valid email address that you will use to log in to your account",
      ),
    username: z
      .string()
      .describe(
        "An email address that you will use to log in to your account. May also be displayed to other players (e.g. leaderboards)",
      ),
    password: z
      .string()
      .describe("A secure password that you will use to log into your account"),
    confirmPassword: z
      .string()
      .describe("Re-type your password to ensure you entered it correctly"),
  })
  .refine((data) => data.password === data.confirmPassword, {
    path: ["confirmPassword"],
    message: "Passwords do not match",
  });

export const signUpResponseSchema = z.object({});

export type SignUpRequest = z.infer<typeof signUpRequestSchema>;
export type SignUpResponse = z.infer<typeof signUpResponseSchema>;
