import { create } from "zustand";
import { persist } from "zustand/middleware";

type UserType = "Guest" | "Basic" | "Admin";

interface UserStore {
  authenticated: boolean;
  sessionExpiry: Date;
  userType: UserType;

  login(userType: UserType, sessionExpiry: Date): void;
  logout(): void;
  isUserType(userType: UserType): boolean;
}

export const useUserStore = create<UserStore>()(
  persist(
    (set, get) => ({
      authenticated: false,
      userType: "Guest",
      sessionExpiry: new Date(0),
      login(userType, sessionExpiry) {
        set({ authenticated: true, userType, sessionExpiry });
      },
      logout() {
        set({
          authenticated: false,
          userType: "Guest",
          sessionExpiry: new Date(0),
        });
      },
      isUserType(userType) {
        const actualUserType = get().userType;
        console.log("Checking user type:", actualUserType);
        return actualUserType === userType;
      },
    }),
    {
      name: "user",
    }
  )
);
